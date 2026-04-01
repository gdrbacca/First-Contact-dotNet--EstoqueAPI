using Estoque.Dominio.Interfaces;
using Shared.Dominio.Enumeradores;
using Shared.Dominio.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estoque.Dominio.Modelos;

public class MovimentoEstoque : EntidadeBase, IDataCriacao
{
    public Guid ProdutoEstoqueId { get; private set; }
    public virtual ProdutoEstoque ProdutoEstoque { get; private set; }
    public TipoMovimentoEstoque Tipo { get; private set; }
    public int? Quantidade { get; private set; }
    public Guid? PedidoExternoId { get; private set; }
    public DateTime DataCriacao { get; set; }

    protected MovimentoEstoque() { }

    public MovimentoEstoque(Guid produtoEstoqueIdParam, TipoMovimentoEstoque tipoParam, int? quantidadeParam, Guid? pedidoExternoId, DateTime dataCriacaoParam)
    {
        ProdutoEstoqueId = produtoEstoqueIdParam;
        Tipo = tipoParam;
        Quantidade = quantidadeParam;
        PedidoExternoId = pedidoExternoId;
        DataCriacao = dataCriacaoParam;
    }

    public void Atualizar() { }
}
