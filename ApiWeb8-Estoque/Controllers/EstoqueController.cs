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
            var result = await produtoEstoque.ObterProdutosEstoqueAsync(cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarProdutoPorId([FromRoute]Guid id, CancellationToken cancellationToken)
        {
            var result = await produtoEstoque.ObterProdutoEstoquePorIdProdutoAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpGet("quantidade/{id}")]
        public async Task<IActionResult> BuscarQuantidadeProdutoPorId([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await produtoEstoque.ObterQuantidadeProdutoAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AlterarEstoque([FromBody] AlterarEstoqueContrato estoque, CancellationToken cancellationToken)
        {
            var retorno = await produtoEstoque.AlterarEstoqueAsync(estoque, cancellationToken);
            return Ok(retorno);
        }

    }
}
