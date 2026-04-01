using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Dominio.Interfaces;

public interface IDataCriacao
{
    DateTime DataCriacao { get; set; }
}