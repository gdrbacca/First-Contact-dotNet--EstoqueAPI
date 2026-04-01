using Estoque.Dominio.Contratos.MovimentoEstoque;
using Estoque.Dominio.Contratos.ProdutoEstoque;
using Shared.Dominio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estoque.Dominio.Interfaces;

public interface IMovimentoEstoqueServico
{
    Task GravarMovimentoEstoqueAsync(CriarMovimentoEstoqueContrato movimento, CancellationToken cancellationToken = default);
}

