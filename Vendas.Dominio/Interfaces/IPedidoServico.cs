using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Dominio.Contratos;
using Vendas.Dominio.Contratos.Pedido;

namespace Vendas.Dominio.Interfaces;

public interface IPedidoServico
{
    Task<List<RetornoBuscarPedidoContrato>?> BuscarTodosPedidos(CancellationToken cancellationToken);
    Task<RetornoBuscarPedidoContrato?> BuscarPedidoPorId(Guid id, CancellationToken cancellationToken);
    Task<RetornoBaseContrato> CriarPedidoAsync(CriarPedidoContrato pedido, CancellationToken cancellationToken);
    Task<RetornoBaseContrato> DeletarPedidoAsync(Guid id, CancellationToken cancellationToken);
}
