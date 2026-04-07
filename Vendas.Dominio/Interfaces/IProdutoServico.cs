using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Dominio.Contratos;
using Vendas.Dominio.Contratos.Produto;

namespace Vendas.Dominio.Interfaces;

public interface IProdutoServico
{
    Task<RetornoBaseContrato> CriarProdutoAsync(CriarProdutoContrato produtoParam, CancellationToken cancellationToken = default);
    Task<RetornoBaseContrato> AtualizarProdutoAsync(Guid id, AtualizarProdutoContrato produtoParam, CancellationToken cancellationToken = default);
    Task<List<RetornoProdutoPorIdContrato>?> ObterTodosProdutosAsync(CancellationToken cancellationToken = default);
    Task<RetornoProdutoPorIdContrato?> ObterProdutoPorIdAsync(Guid produtoId, CancellationToken cancellationToken = default);
}
