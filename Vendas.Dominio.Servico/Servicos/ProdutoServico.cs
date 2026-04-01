using Microsoft.EntityFrameworkCore;
using Shared.Dominio.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Dominio.Contratos.Produto;
using Vendas.Dominio.Interfaces;
using Vendas.Infra.Database.Contextos;

namespace Vendas.Dominio.Servico.Servicos;

public class ProdutoServico : IProdutoServico
{
    private readonly AppDbContext _context;

    public ProdutoServico(AppDbContext context)
    {
        _context = context;
    }

    public async Task AtualizarProdutoAsync(Guid id, AtualizarProdutoContrato produtoParam, CancellationToken cancellationToken = default)
    {
        var produto = await _context.Produtos
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();

        if (produto is null)
        {
            throw new Exception("Produto não encontrado.");
        }

        produto.Atualizar(produtoParam.Nome, produtoParam.Descricao, produtoParam.Preco);
        _context.Produtos.Update(produto);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task CriarProdutoAsync(CriarProdutoContrato produtoParam, CancellationToken cancellationToken = default)
    {
        var produto = new ProdutoEntidade(produtoParam.Nome, produtoParam.Descricao, produtoParam.Preco);

        await _context.Produtos.AddAsync(produto);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<RetornoProdutoPorIdContrato> ObterProdutoPorIdAsync(Guid produtoId, CancellationToken cancellationToken = default)
    {
        var produto = await _context.Produtos
            .Where(x => x.Id == produtoId)
            .Select(y => new RetornoProdutoPorIdContrato(
                   y.Id,
                   y.Nome,
                   y.Descricao,
                   y.ValorUnitario
             ))
            .FirstOrDefaultAsync(cancellationToken);

        if (produto is null)
        {
            throw new Exception("Produto não encontrado");
        }

        return produto;
    }

    public async Task<List<RetornoProdutoPorIdContrato>> ObterTodosProdutosAsync(CancellationToken cancellationToken = default)
    {
        var retorno = await _context.Produtos.Select(x => new RetornoProdutoPorIdContrato(
                x.Id,
                x.Nome,
                x.Descricao,
                x.ValorUnitario
            ))
        .ToListAsync(cancellationToken);

        if (retorno == null || !retorno.Any())
        {
            return new List<RetornoProdutoPorIdContrato>();
        }

        return retorno;
    }
}
