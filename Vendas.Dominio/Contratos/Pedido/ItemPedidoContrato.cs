using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Dominio.Contratos.Pedido;

public record ItemPedidoContrato(
    /*Guid? idPedido,*/
    Guid idProduto,
    int quantidade,
    decimal valorTotal);