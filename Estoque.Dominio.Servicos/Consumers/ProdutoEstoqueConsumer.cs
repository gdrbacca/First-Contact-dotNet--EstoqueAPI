using Estoque.Dominio.Interfaces;
using Estoque.Infra.Database.Contextos;
using MassTransit;
using Mensageria.Contratos.Contratos;
using Shared.Dominio.Contratos;
using Shared.Dominio.Enumeradores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estoque.Dominio.Servicos.Consumers;

public class PedidoCriadoConsumer : IConsumer<PedidoCriadoEvent>
{
    private readonly IProdutoEstoqueServico _produtoEstoqueServico;

    public PedidoCriadoConsumer(IProdutoEstoqueServico produtoEstoqueServico)
    {
        _produtoEstoqueServico = produtoEstoqueServico;
    }

    public async Task Consume(ConsumeContext<PedidoCriadoEvent> context)
    {
        var mensagem = context.Message;

        foreach (var item in mensagem.itens)
        {
            var alterarEstoque = new AlterarEstoqueContrato(item.idProduto, TipoMovimentoEstoque.SaidaProduto, item.quantidade, null, mensagem.pedidoId);

            await _produtoEstoqueServico.AlterarEstoqueAsync(alterarEstoque);
        }
    }

}
