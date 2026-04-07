using Shared.Dominio.Contratos;
using Estoque.Dominio.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiWeb8_Estoque.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstoqueController : ControllerBase
    {
        public readonly IProdutoEstoqueServico produtoEstoque;
        public readonly IMovimentoEstoqueServico movimentoEstoque; //teste

        public EstoqueController(IProdutoEstoqueServico servico, IMovimentoEstoqueServico movimento)
        {
            produtoEstoque = servico;
            movimentoEstoque = movimento;
        }

        [HttpGet]
        public async Task<IActionResult> BuscarProdutos(CancellationToken cancellationToken)
        {
            try
            {
                var result = await produtoEstoque.ObterProdutosEstoqueAsync(cancellationToken);
                if (result == null || !result.Any())
                {
                    return NoContent();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao buscar produtos: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarProdutoPorId([FromRoute]Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await produtoEstoque.ObterProdutoEstoquePorIdProdutoAsync(id, cancellationToken);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao buscar produto por ID: {ex.Message}");
            }
        }

        [HttpGet("quantidade/{id}")]
        public async Task<IActionResult> BuscarQuantidadeProdutoPorId([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await produtoEstoque.ObterQuantidadeProdutoAsync(id, cancellationToken);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao buscar quantidade do produto por ID: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AlterarEstoque([FromBody] AlterarEstoqueContrato estoque, CancellationToken cancellationToken)
        {
            try
            {
                var retorno = await produtoEstoque.AlterarEstoqueAsync(estoque, cancellationToken);

                if (retorno.sucesso)
                {
                    return Ok(retorno);
                }
                else
                {
                    return BadRequest(retorno.mensagemErro);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao alterar estoque: {ex.Message}");
            }
        }

    }
}
