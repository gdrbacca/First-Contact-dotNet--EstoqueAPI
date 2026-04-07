using Estoque.Dominio.Contratos.ProdutoEstoque;
using Estoque.Dominio.Interfaces;
using Estoque.Infra.Database.Contextos;
using Microsoft.EntityFrameworkCore;
using Shared.Dominio.Modelos;
using Shared.Dominio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estoque.Dominio.Contratos.MovimentoEstoque;
using Shared.Dominio.Enumeradores;
using System.Diagnostics;

namespace Estoque.Dominio.Servicos.Servicos;

public class ProdutoEstoqueServico : IProdutoEstoqueServico
{
    private readonly AppDbContextEstoque _context;
    private readonly IMovimentoEstoqueServico _movimentoEstoque;

    public ProdutoEstoqueServico(AppDbContextEstoque context, IMovimentoEstoqueServico movimento)
    {
        _context = context;
        _movimentoEstoque = movimento;
    }

    public async Task<List<ProdutoEstoqueComProdutoContrato>?> ObterProdutosEstoqueAsync(CancellationToken cancellationToken = default)
    {
        var resultado = await _context.ProdutoEstoque
            .ComProduto(_context)
            .ToListAsync(cancellationToken);

        if (resultado is null || !resultado.Any())
        {
            return null;
        }

        return resultado;
    }

    public async Task<ProdutoEstoqueComProdutoContrato?> ObterProdutoEstoquePorIdProdutoAsync(Guid idProduto, CancellationToken cancellationToken = default)
    {
        var resultado = await _context.ProdutoEstoque
            .Where(x => x.ProdutoId == idProduto)
            .ComProduto(_context)
            .FirstOrDefaultAsync(cancellationToken);

        if (resultado is null)
        {
            return null;
        }

        return resultado;
    }

    public async Task<ProdutoEstoqueQuantidadeContrato?> ObterQuantidadeProdutoAsync(Guid idProduto, CancellationToken cancellationToken = default)
    {
        var resultado = await _context.ProdutoEstoque
            .Where(pe => pe.ProdutoId == idProduto)
            .Select(pe => new ProdutoEstoqueQuantidadeContrato(pe.QuantidadeDisponivel, pe.EstoqueMinimo))
            .FirstOrDefaultAsync(cancellationToken);

        if (resultado is null)
        {
            return null;
        }

        return resultado;
    }

    public async Task<RetornoAlterarEstoqueContrato> AlterarEstoqueAsync(AlterarEstoqueContrato produtoEstoque, CancellationToken cancellationToken = default)
    {
        try
        {
            switch (produtoEstoque.tipoMovimento)
            {
                case TipoMovimentoEstoque.CriacaoProduto:
                    var newProdutoEstoque = new ProdutoEstoque(produtoEstoque.produtoId, produtoEstoque.quantidade, produtoEstoque.estoqueMinimo ?? 0, DateTime.MinValue);

                    _context.ProdutoEstoque.Add(newProdutoEstoque);
                    await _context.SaveChangesAsync(cancellationToken);

                    var newMovimentoEstoque = new CriarMovimentoEstoqueContrato(
                        newProdutoEstoque.Id, TipoMovimentoEstoque.CriacaoProduto, newProdutoEstoque.QuantidadeDisponivel, null);

                    await _movimentoEstoque.GravarMovimentoEstoqueAsync(newMovimentoEstoque, cancellationToken);

                    return new RetornoAlterarEstoqueContrato(true, "Produto criado no estoque.");

                case TipoMovimentoEstoque.SaidaProduto:
                    var estoque = await this.ObterProdutoEstoquePorIdProdutoAsync(produtoEstoque.produtoId, cancellationToken);
                    if (estoque == null) throw new Exception("Produto em estoque não encontrado.");
                    int novaQuantidade = estoque.QuantidadeDisponivel - produtoEstoque.quantidade;
                    if (novaQuantidade < 0) throw new Exception("Quantidade em estoque insuficiente para a saída.");
                    DateTime dataAtualizacao = DateTime.UtcNow;

                    await _context.ProdutoEstoque
                        .Where(pe => pe.Id == estoque.Id)
                        .ExecuteUpdateAsync(setter => setter
                            .SetProperty(p => p.QuantidadeDisponivel, novaQuantidade)
                            .SetProperty(p => p.UltimaAtualizacao, dataAtualizacao));

                    var newMovimentoEstoqueSaida = new CriarMovimentoEstoqueContrato(
                        estoque.Id, TipoMovimentoEstoque.SaidaProduto, produtoEstoque.quantidade, produtoEstoque.pedidoId);

                    await _movimentoEstoque.GravarMovimentoEstoqueAsync(newMovimentoEstoqueSaida, cancellationToken);

                    return new RetornoAlterarEstoqueContrato(true, "Quantidade subtraída do estoque.");

                case TipoMovimentoEstoque.EntradaProduto:
                    var estoqueEntrada = await this.ObterProdutoEstoquePorIdProdutoAsync(produtoEstoque.produtoId, cancellationToken);
                    if (estoqueEntrada == null) throw new Exception("Produto em estoque não encontrado.");
                    int novaQuantidadeEntrada = estoqueEntrada.QuantidadeDisponivel + produtoEstoque.quantidade;
                    DateTime dataAtualizacaoEntrada = DateTime.UtcNow;

                    await _context.ProdutoEstoque
                        .Where(pe => pe.Id == estoqueEntrada.Id)
                        .ExecuteUpdateAsync(setter => setter
                            .SetProperty(p => p.QuantidadeDisponivel, novaQuantidadeEntrada)
                            .SetProperty(p => p.UltimaAtualizacao, dataAtualizacaoEntrada));

                    var newMovimentoEstoqueEntrada = new CriarMovimentoEstoqueContrato(
                        estoqueEntrada.Id, TipoMovimentoEstoque.EntradaProduto, produtoEstoque.quantidade, null);

                    await _movimentoEstoque.GravarMovimentoEstoqueAsync(newMovimentoEstoqueEntrada, cancellationToken);

                    return new RetornoAlterarEstoqueContrato(true, "Quantidade atribuída ao estoque.");

                case TipoMovimentoEstoque.ExclusaoProduto:
                    var estoqueExclusao = await this.ObterProdutoEstoquePorIdProdutoAsync(produtoEstoque.produtoId, cancellationToken);
                    if (estoqueExclusao == null) throw new Exception("Produto em estoque não encontrado.");
                    var produtoEstoqueToDelete = await _context.ProdutoEstoque
                        .Where(pe => pe.Id == estoqueExclusao.Id)
                        .FirstOrDefaultAsync(cancellationToken);

                    if (produtoEstoqueToDelete is null)
                    {
                        throw new Exception("Estoque não encontrado.");
                    }

                    _context.Remove(produtoEstoqueToDelete);
                    await _context.SaveChangesAsync(cancellationToken);

                    return new RetornoAlterarEstoqueContrato(true, "Produto excluído do estoque.");

                default:
                    return new RetornoAlterarEstoqueContrato(false, "Tipo de movimento de estoque inválido.");
            }
        }
        catch (Exception ex)
        {
            return new RetornoAlterarEstoqueContrato(false, ex.Message);
        }
    }
}

// classe com metodo estatico para extensão
public static class ProdutoEstoqueExtensions
{
    public static IQueryable<ProdutoEstoqueComProdutoContrato> ComProduto(
        this IQueryable<ProdutoEstoque> query,
        AppDbContextEstoque context)
    {
        return query.Join(
            context.Set<ProdutoEntidade>(),
            pe => pe.ProdutoId,
            p => p.Id,
            (pe, p) => new ProdutoEstoqueComProdutoContrato
            (
                pe.Id,
                pe.QuantidadeDisponivel,
                pe.EstoqueMinimo,
                pe.ProdutoId,
                p.Nome,
                p.Descricao
            ));
    }
}
