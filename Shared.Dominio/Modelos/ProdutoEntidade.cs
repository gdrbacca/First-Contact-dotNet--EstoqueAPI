using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dominio.Modelos;

public class ProdutoEntidade : EntidadeBase
{
    public string Nome { get; private set; }
    public string Descricao { get; private set; }
    public decimal ValorUnitario { get; private set; }

    protected ProdutoEntidade() { }

    public ProdutoEntidade(string nomeParam, string descricaoParam, decimal valorUnitarioParam)
    {
        Nome = nomeParam;
        Descricao = descricaoParam;
        ValorUnitario = valorUnitarioParam;
    }

    public void Atualizar(string nomeParam, string descricaoParam, decimal valorUnitarioParam) { 
        
        if (!string.IsNullOrEmpty(nomeParam))
        {
            this.Nome = nomeParam;
        }

        if (!string.IsNullOrEmpty(descricaoParam))
        { 
            this.Descricao = descricaoParam;
        }

        if (valorUnitarioParam > 0.0m)
        {
            this.ValorUnitario = valorUnitarioParam;
        }
    }
}
