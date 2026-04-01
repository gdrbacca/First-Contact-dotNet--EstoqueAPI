using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mensageria.Contratos.Contratos;

public record PedidoCriadoEvent (
    Guid? pedidoId,
    DateTime dataCriacao,
    List<ItemPedidoEvent> itens
);

public record ItemPedidoEvent(
    Guid idProduto,
    int quantidade
);