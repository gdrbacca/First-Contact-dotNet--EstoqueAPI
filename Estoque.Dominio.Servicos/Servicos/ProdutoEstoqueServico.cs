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

    public async Task<List<ProdutoEstoqueComProdutoContrato>> ObterProdutosEstoqueAsync(CancellationToken cancellationToken = default)
    {
        var resultado = await _context.ProdutoEstoque
            .ComProduto(_context)
            .ToListAsync(cancellationToken);

        if (resultado is null)
        {
            throw new Exception("Estoque não encontrado.");
        }

        return resultado;
    }

    public async Task<ProdutoEstoqueComProdutoContrato> ObterProdutoEstoquePorIdProdutoAsync(Guid idProduto, CancellationToken cancellationToken = default)
    {
        var resultado = await _context.ProdutoEstoque
            .Where(x => x.ProdutoId == idProduto)
            .ComProduto(_context)
            .FirstOrDefaultAsync(cancellationToken);

        if (resultado is null)
        {
            throw new Exception("Estoque não encontrado.");
        }

        return resultado;
    }

    public async Task<ProdutoEstoqueQuantidadeContrato> ObterQuantidadeProdutoAsync(Guid idProduto, CancellationToken cancellationToken = default)
    {
        var resultado = await _context.ProdutoEstoque
            .Where(pe => pe.ProdutoId == idProduto)
            .Select(pe => new ProdutoEstoqueQuantidadeContrato(pe.QuantidadeDisponivel, pe.EstoqueMinimo))
            .FirstOrDefaultAsync(cancellationToken);

        if (resultado is null)
        {
            throw new Exception("Estoque não encontrado.");
        }

        return resultado;
    }

    public async Task<RetornoAlterarEstoqueContrato> AlterarEstoqueAsync(AlterarEstoqueContrato produtoEstoque, CancellationToken cancellationToken = default)
    {
        ProdutoEstoque newProdutoEstoque;
        CriarMovimentoEstoqueContrato newMovimentoEstoque;
        string mensagemRetorno = "";

        switch (produtoEstoque.tipoMovimento)
        {
            case TipoMovimentoEstoque.CriacaoProduto:
                newProdutoEstoque = new ProdutoEstoque(produtoEstoque.produtoId, produtoEstoque.quantidade, produtoEstoque.estoqueMinimo ?? 0, DateTime.MinValue);

                _context.ProdutoEstoque.Add(newProdutoEstoque);
                await _context.SaveChangesAsync(cancellationToken);

                newMovimentoEstoque = new CriarMovimentoEstoqueContrato(
                    newProdutoEstoque.Id, TipoMovimentoEstoque.CriacaoProduto, newProdutoEstoque.QuantidadeDisponivel, null);

                await _movimentoEstoque.GravarMovimentoEstoqueAsync(newMovimentoEstoque, cancellationToken);

                mensagemRetorno = "Produto criado no estoque.";
            break;

            case TipoMovimentoEstoque.SaidaProduto:
                var estoque = await this.ObterProdutoEstoquePorIdProdutoAsync(produtoEstoque.produtoId, cancellationToken);
                int novaQuantidade = estoque.QuantidadeDisponivel - produtoEstoque.quantidade;
                DateTime dataAtualizacao = DateTime.UtcNow;

                await _context.ProdutoEstoque
                    .Where(pe => pe.Id == estoque.Id)
                    .ExecuteUpdateAsync(setter => setter
                        .SetProperty(p => p.QuantidadeDisponivel, novaQuantidade)
                        .SetProperty(p => p.UltimaAtualizacao, dataAtualizacao));

                newMovimentoEstoque = new CriarMovimentoEstoqueContrato(
                    estoque.Id, TipoMovimentoEstoque.SaidaProduto, produtoEstoque.quantidade, produtoEstoque.pedidoId);

                await _movimentoEstoque.GravarMovimentoEstoqueAsync(newMovimentoEstoque, cancellationToken);

                mensagemRetorno = "Quantidade subtraída do estoque.";
                break;

            case TipoMovimentoEstoque.EntradaProduto:
                var estoque2 = await this.ObterProdutoEstoquePorIdProdutoAsync(produtoEstoque.produtoId, cancellationToken);
                int novaQuantidade2 = estoque2.QuantidadeDisponivel + produtoEstoque.quantidade;
                DateTime dataAtualizacao2 = DateTime.UtcNow;

                await _context.ProdutoEstoque
                    .Where(pe => pe.Id == estoque2.Id)
                    .ExecuteUpdateAsync(setter => setter
                        .SetProperty(p => p.QuantidadeDisponivel, novaQuantidade2)
                        .SetProperty(p => p.UltimaAtualizacao, dataAtualizacao2));

                newMovimentoEstoque = new CriarMovimentoEstoqueContrato(
                    estoque2.Id, TipoMovimentoEstoque.EntradaProduto, produtoEstoque.quantidade, null);

                await _movimentoEstoque.GravarMovimentoEstoqueAsync(newMovimentoEstoque, cancellationToken);

                mensagemRetorno = "Quantidade atribuída ao estoque.";
                break;

            case TipoMovimentoEstoque.ExclusaoProduto:
                var estoque3 = await this.ObterProdutoEstoquePorIdProdutoAsync(produtoEstoque.produtoId, cancellationToken);
                var produtoEstoqueToDelete = await _context.ProdutoEstoque
                    .Where(pe => pe.Id == estoque3.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                if (produtoEstoqueToDelete is null)
                {
                    throw new Exception("Estoque não encontrado.");
                }

                _context.Remove(produtoEstoqueToDelete);
                await _context.SaveChangesAsync(cancellationToken);

                mensagemRetorno = "Produto excluído do estoque.";
                break;
            default: break;
        }

        return new RetornoAlterarEstoqueContrato(mensagemRetorno);
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

//var estoques = await _context.ProdutoEstoque
//    .Join(_context.Set<ProdutoEntidade>(),
//          pe => pe.ProdutoId,
//          p => p.Id,
//          (pe, p) => new { ProdutoEstoque = pe, Produto = p })
//    .Where(x => x.ProdutoEstoque.Quantidade < 10)
//    .Select(x => new
//    {
//        x.ProdutoEstoque.Id,
//        ProdutoNome = x.Produto.Nome,
//        x.ProdutoEstoque.Quantidade
//    })
//    .ToListAsync();


//var estoques = await _context.ProdutoEstoque
//    .Include(pe => pe.Produto)                  // ← usa a navigation property
//    .Where(pe => pe.Quantidade < 10)
//    .ToListAsync();


//var bateria = await _context.Baterias
//            .Where(x => x.Id == bateriaId)
//            .Select(x => new RetornoBateriaContrato(
//                x.Id,
//                x.Descricao,
//                x.DataHora,
//                x.Equipes.Select(x => new Contratos.Equipe.ObterEquipeContrato(x.Nome, x.Atletas, x.Raia)).ToList()))
//            .FirstOrDefaultAsync(cancellationToken);


//var resultado = await _context.ProdutoEstoque
//    .Join(_context.Set<ProdutoEntidade>(),
//          pe => pe.ProdutoId,
//          p => p.Id,
//          (pe, p) => new
//          {
//              pe.Id,
//              pe.Quantidade,
//              ProdutoNome = p.Nome,
//              pe.ProdutoId
//          })
//    .Where(x => x.Quantidade < 10)
//    .ToListAsync();