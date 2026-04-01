using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Dominio.Contratos.Produto;

public record CriarProdutoContrato(
    string Nome,
    string Descricao,
    decimal Preco);
