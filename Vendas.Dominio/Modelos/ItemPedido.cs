using Shared.Dominio.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Dominio.Modelos;

public class ItemPedido : EntidadeBase
{
    public Guid PedidoId { get; private set; }
    public virtual Pedido Pedido { get; private set; }
    public Guid ProdutoId { get; private set; }
    public virtual ProdutoEntidade Produto { get; private set; }
    public int Quantidade { get; private set; }
    public decimal ValorTotal { get; private set; }

    protected ItemPedido() { }

    public ItemPedido(Guid pedidoIdParam, Guid produtoIdParam, int quantidadeParam, decimal valorTotalParam)
    {
        PedidoId = pedidoIdParam;
        ProdutoId = produtoIdParam;
        Quantidade = quantidadeParam;
        ValorTotal = valorTotalParam;
    }

    public ItemPedido(Guid produtoIdParam, int quantidadeParam, decimal valorTotalParam)
    {
        ProdutoId = produtoIdParam;
        Quantidade = quantidadeParam;
        ValorTotal = valorTotalParam;
    }

    public void Atualizar() { }
}
