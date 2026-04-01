using Shared.Dominio.Enumeradores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estoque.Dominio.Contratos.MovimentoEstoque;

public record CriarMovimentoEstoqueContrato
(
    Guid produtoEstoqueId,
    TipoMovimentoEstoque tipo,
    int? quantidade,
    Guid? pedidoExternoId
);