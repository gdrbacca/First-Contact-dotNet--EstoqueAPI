using Estoque.Dominio.Contratos.MovimentoEstoque;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estoque.Dominio.Interfaces;

public interface IDataCriacao
{
    DateTime DataCriacao { get; set; }
}

