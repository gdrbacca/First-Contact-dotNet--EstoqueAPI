using Estoque.Dominio.Contratos.ProdutoEstoque;
using Shared.Dominio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estoque.Dominio.Interfaces;

public interface IProdutoEstoqueServico
{
    Task<List<ProdutoEstoqueComProdutoContrato>?> ObterProdutosEstoqueAsync(CancellationToken cancellationToken = default);
    Task<ProdutoEstoqueComProdutoContrato?> ObterProdutoEstoquePorIdProdutoAsync(Guid idProduto, CancellationToken cancellationToken = default);
    Task<ProdutoEstoqueQuantidadeContrato?> ObterQuantidadeProdutoAsync(Guid idProduto, CancellationToken cancellationToken = default);
    Task<RetornoAlterarEstoqueContrato> AlterarEstoqueAsync(AlterarEstoqueContrato produtoEstoque, CancellationToken cancellationToken = default);
}
