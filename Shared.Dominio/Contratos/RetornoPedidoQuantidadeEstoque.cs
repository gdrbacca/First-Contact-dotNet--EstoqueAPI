using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dominio.Contratos;

public record ProdutoEstoqueQuantidadeContrato
(
    int QuantidadeDisponivel,
    int QuantidadeMinima
);