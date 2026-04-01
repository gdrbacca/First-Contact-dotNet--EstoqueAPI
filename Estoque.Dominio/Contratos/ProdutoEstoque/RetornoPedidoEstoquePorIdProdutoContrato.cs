using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estoque.Dominio.Contratos.ProdutoEstoque;

public record ProdutoEstoqueComProdutoContrato
(
    Guid Id,
    int QuantidadeDisponivel,
    int QuantidadeMinima,
    Guid ProdutoId,
    string ProdutoNome,
    string ProdutoDescricao
);