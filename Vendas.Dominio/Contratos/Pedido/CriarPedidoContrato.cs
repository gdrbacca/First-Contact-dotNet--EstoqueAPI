using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Dominio.Contratos.Pedido;

public record CriarPedidoContrato(
    decimal total,
    DateTime dataCriacao,
    IList<ItemPedidoContrato> itensPedido);