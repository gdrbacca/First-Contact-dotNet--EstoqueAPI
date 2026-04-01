using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Dominio.Contratos.Produto;
using Vendas.Dominio.Modelos;

namespace Vendas.Dominio.Contratos.Pedido;

public record RetornoBuscarPedidoContrato
(
    Guid idPedido,
    decimal totalPedido,
    DateTime dataCriacao,
    List<RetornoProdutoPorIdContrato> itensPedido
);