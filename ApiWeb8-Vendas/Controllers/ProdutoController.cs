using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vendas.Dominio.Contratos.Produto;
using Vendas.Dominio.Interfaces;

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
        var result = await produtoServico.ObterTodosProdutosAsync(cancellationToken);
        return Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarProdutoPorId(Guid id, CancellationToken cancellationToken)
    {
        var result = await produtoServico.ObterProdutoPorIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CriarProduto([FromBody] CriarProdutoContrato produto, CancellationToken cancellationToken)
    {
        await produtoServico.CriarProdutoAsync(produto, cancellationToken);
        return Ok(true);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AlteraProduto(Guid id,[FromBody] AtualizarProdutoContrato produto, CancellationToken cancellationToken)
    {
        await produtoServico.AtualizarProdutoAsync(id, produto, cancellationToken);
        return Ok(true);
    }
}
