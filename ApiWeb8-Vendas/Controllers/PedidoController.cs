using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vendas.Dominio.Contratos;
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
            try
            {
                var resultado = await pedidoServico.CriarPedidoAsync(pedido, cancellation);
                if (resultado.Sucesso)
                {
                    return CreatedAtAction(nameof(ObterPedidoPorId), new { id = resultado.Id }, resultado);
                }
                return BadRequest(resultado.MensagemErro);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarPedido(Guid id, CancellationToken cancellation)
        {
            try
            {
                var resultado = await pedidoServico.DeletarPedidoAsync(id, cancellation);
                if (resultado.Sucesso)
                {
                    return Ok();
                }
                return BadRequest(resultado.MensagemErro);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPedidoPorId(Guid id, CancellationToken cancellation)
        {
            try
            {
                var resultado = await pedidoServico.BuscarPedidoPorId(id, cancellation);
                if (resultado is null)
                {
                    return NotFound();
                }
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodosPedidos(CancellationToken cancellation)
        {
            try
            {
                var resultado = await pedidoServico.BuscarTodosPedidos(cancellation);
                if (resultado is null || !resultado.Any())
                {
                    return NoContent();
                }
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
