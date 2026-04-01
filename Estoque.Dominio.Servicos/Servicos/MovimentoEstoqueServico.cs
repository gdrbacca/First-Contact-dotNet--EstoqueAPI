using Estoque.Dominio.Contratos.MovimentoEstoque;
using Estoque.Dominio.Contratos.ProdutoEstoque;
using Estoque.Dominio.Interfaces;
using Estoque.Dominio.Modelos;
using Estoque.Infra.Database.Contextos;
using Shared.Dominio.Contratos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estoque.Dominio.Servicos.Servicos;

public class MovimentoEstoqueServico : IMovimentoEstoqueServico
{
    private readonly AppDbContextEstoque _context;

    public MovimentoEstoqueServico(AppDbContextEstoque context)
    {
        _context = context;
    }

    public async Task GravarMovimentoEstoqueAsync(CriarMovimentoEstoqueContrato movimento, CancellationToken cancellationToken = default)
    {
        Debug.WriteLine("Chegou aqui");
        Debug.WriteLine(movimento);

        var movimentoEstoque = new MovimentoEstoque(movimento.produtoEstoqueId, movimento.tipo, movimento.quantidade, movimento.pedidoExternoId, DateTime.MinValue);

        _context.MovimentoEstoque.Add(movimentoEstoque);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
