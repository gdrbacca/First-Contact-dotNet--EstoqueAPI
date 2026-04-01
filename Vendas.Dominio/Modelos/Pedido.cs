using Shared.Dominio.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Dominio.Interfaces;

namespace Vendas.Dominio.Modelos;

public class Pedido : EntidadeBase, IDataCriacao
{
    public decimal Total { get; private set; }
    public IList<ItemPedido> ItensPedido { get; private set; } = new List<ItemPedido>();

    public DateTime DataCriacao { get; set; }

    protected Pedido() { }

    public Pedido(decimal totalParam, DateTime dataCriacaoParam, IList<ItemPedido> itensPedidoParam)
    {
        Total = totalParam;
        DataCriacao = dataCriacaoParam;
        ItensPedido = itensPedidoParam;
    }

    public void Atualizar() { }

}
