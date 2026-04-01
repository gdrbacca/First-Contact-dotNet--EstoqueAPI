using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vendas.Dominio.Contratos.Pedido;
using Vendas.Dominio.Interfaces;

namespace ApiWeb8_Vendas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        public readonly IPedidoServico pedidoServico;

        public PedidoController(IPedidoServico servico)
        {
            pedidoServico = servico;
        }

        [HttpPost]
        public async Task<IActionResult> CriarPedido([FromBody] CriarPedidoContrato pedido, CancellationToken cancellation)
        {
            await pedidoServico.CriarPedidoAsync(pedido, cancellation);
            return Ok(true);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarPedido(Guid id, CancellationToken cancellation)
        {
            await pedidoServico.DeletarPedidoAsync(id, cancellation);
            return Ok(true);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPedidoPorId(Guid id, CancellationToken cancellation)
        {
            var resultado = await pedidoServico.BuscarPedidoPorId(id, cancellation);
            return Ok(resultado);
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodosPedidos(CancellationToken cancellation)
        {
            var resultado = await pedidoServico.BuscarTodosPedidos(cancellation);
            return Ok(resultado);
        }


    }
}
