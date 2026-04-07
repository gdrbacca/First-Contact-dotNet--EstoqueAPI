using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vendas.Dominio.Contratos.Produto;
using Vendas.Dominio.Interfaces;
using Vendas.Dominio.Contratos;

namespace ApiWeb8_Vendas.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutoController : ControllerBase
{
    public readonly IProdutoServico produtoServico;

    public ProdutoController(IProdutoServico servico)
    {
        produtoServico = servico;
    }

    [HttpGet]
    public async Task<IActionResult> BuscarTodosProdutos(CancellationToken cancellationToken)
    {
        try
        {
            var result = await produtoServico.ObterTodosProdutosAsync(cancellationToken);
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
    public async Task<IActionResult> BuscarProdutoPorId(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await produtoServico.ObterProdutoPorIdAsync(id, cancellationToken);
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

    [HttpPost]
    public async Task<IActionResult> CriarProduto([FromBody] CriarProdutoContrato produto, CancellationToken cancellationToken)
    {
        try
        {
            var retorno = await produtoServico.CriarProdutoAsync(produto, cancellationToken);
            if (retorno.Sucesso)
            {
                return CreatedAtAction(nameof(BuscarProdutoPorId), new { id = retorno.Id }, retorno);
            }
            else
            {
                return BadRequest(retorno.MensagemErro);
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao criar produto: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AlteraProduto(Guid id,[FromBody] AtualizarProdutoContrato produto, CancellationToken cancellationToken)
    {
        try
        {
            var retorno = await produtoServico.AtualizarProdutoAsync(id, produto, cancellationToken);
            if (retorno.Sucesso)
            {
                return Ok(retorno);
            }
            else
            {
                return BadRequest(retorno.MensagemErro);
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao alterar produto: {ex.Message}");
        }
    }
}
