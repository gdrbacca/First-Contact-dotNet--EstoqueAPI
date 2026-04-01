using Estoque.Dominio.Interfaces;
using Shared.Dominio.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ProdutoEstoque : EntidadeBase, IUltimaAtualizacao
{
    public Guid ProdutoId { get; private set; }
    // public virtual ProdutoEntidade Produto { get; private set; }
    public int QuantidadeDisponivel { get; private set; }
    public int EstoqueMinimo { get; private set; }
    public DateTime UltimaAtualizacao { get; set; }

    //criar um isDeleted
    protected ProdutoEstoque() { }

    public ProdutoEstoque(Guid produtoId, int quantidadeDisponivelParam, int estoqueMinimoParam, DateTime ultimaAtualizacaoParam)
    {
        ProdutoId = produtoId;
        QuantidadeDisponivel = quantidadeDisponivelParam;
        EstoqueMinimo = estoqueMinimoParam;
        UltimaAtualizacao = ultimaAtualizacaoParam;
    }

    public void Atualizar() { }
}
