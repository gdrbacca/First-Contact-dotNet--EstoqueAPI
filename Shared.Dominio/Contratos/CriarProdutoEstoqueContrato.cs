using Shared.Dominio.Enumeradores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dominio.Contratos;

public record AlterarEstoqueContrato(
    Guid produtoId,
    TipoMovimentoEstoque tipoMovimento,
    int quantidade,
    int? estoqueMinimo,
    Guid? pedidoId);