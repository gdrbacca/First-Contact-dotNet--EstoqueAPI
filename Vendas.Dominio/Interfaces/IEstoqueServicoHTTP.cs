using Shared.Dominio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Dominio.Interfaces;

public interface IEstoqueServicoHTTP
{
    Task<ProdutoEstoqueQuantidadeContrato> ObterQuantidadeEstoqueProdutoAsync(Guid produtoId, CancellationToken cancellation = default);
}
