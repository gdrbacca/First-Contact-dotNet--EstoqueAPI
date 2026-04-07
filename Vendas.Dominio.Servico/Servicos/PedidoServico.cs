using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Vendas.Dominio.Contratos;
using Vendas.Dominio.Contratos.Pedido;
using Vendas.Dominio.Contratos.Produto;
using Vendas.Dominio.Interfaces;
using Vendas.Dominio.Modelos;
using Vendas.Infra.Database.Contextos;
using Mensageria.Contratos.Contratos;

namespace Vendas.Dominio.Servico.Servicos;

public class PedidoServico : IPedidoServico
{
    private readonly AppDbContext _context;
    private readonly IEstoqueServicoHTTP _httpClient;
    private readonly IPublishEndpoint _publishEndpoint;

    public PedidoServico(AppDbContext context, IEstoqueServicoHTTP httpClient, IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _httpClient = httpClient;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<RetornoBuscarPedidoContrato?> BuscarPedidoPorId(Guid id, CancellationToken cancellationToken)
    {
        var retorno = await _context.Pedidos
            .Where(p => p.Id == id)
            .Select(p => new RetornoBuscarPedidoContrato(
               p.Id,
                p.Total,
                p.DataCriacao,
                p.ItensPedido.Select(ip => new RetornoProdutoPorIdContrato(
                    ip.ProdutoId,
                    ip.Produto!.Nome,     
                    ip.Produto!.Descricao,
                    ip.Produto!.ValorUnitario
                )).ToList()
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return retorno;
    }

    public async Task<List<RetornoBuscarPedidoContrato>?> BuscarTodosPedidos(CancellationToken cancellationToken)
    {
        var retorno = await _context.Pedidos
            .Select(p => new RetornoBuscarPedidoContrato(
                p.Id,
                p.Total,
                p.DataCriacao,
                p.ItensPedido.Select(ip => new RetornoProdutoPorIdContrato(
                    ip.ProdutoId,
                    ip.Produto!.Nome,
                    ip.Produto!.Descricao,
                    ip.Produto!.ValorUnitario
                )).ToList()
            ))
            .ToListAsync(cancellationToken);

        return retorno;
    }

    public async Task<RetornoBaseContrato> CriarPedidoAsync(CriarPedidoContrato pedidoParam, CancellationToken cancellationToken)
    {
        try
        {
            var listItens = new List<ItemPedido>(pedidoParam.itensPedido.Count());
            foreach (var item in pedidoParam.itensPedido)
            {
                listItens.Add(new ItemPedido(item.idProduto, item.quantidade, item.valorTotal)); 
            }

            for (int i = 0; i < listItens.Count(); i++)
            {
                Guid idProduto = listItens[i].ProdutoId;
                var quantidadeEstoque = await _httpClient.ObterQuantidadeEstoqueProdutoAsync(idProduto, cancellationToken);

                if ((quantidadeEstoque.QuantidadeDisponivel < quantidadeEstoque.QuantidadeMinima) || (listItens[i].Quantidade >= quantidadeEstoque.QuantidadeDisponivel))
                {
                    return new RetornoBaseContrato(true, "Produto com estoque insuficiente.", idProduto);
                }
            }
            var pedido = new Pedido(pedidoParam.total, pedidoParam.dataCriacao, listItens);

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync(cancellationToken); // aqui ele ja adiciona os itens e preenche os id's

            var pedidoCriadoEvent = new PedidoCriadoEvent(
                pedido.Id,
                pedido.DataCriacao,
                pedido.ItensPedido.Select(i => new ItemPedidoEvent(
                    i.ProdutoId,
                    i.Quantidade
                )).ToList()
            );

            await _publishEndpoint.Publish(pedidoCriadoEvent, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken); // segundo saveAsync para validar o publish
            return new RetornoBaseContrato(true, "Pedido criado com sucesso.", pedido.Id);
        }
        catch (Exception ex)
        {
            return new RetornoBaseContrato(false, ex.Message);
        }
    }

    public async Task<RetornoBaseContrato> DeletarPedidoAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var pedido = await _context.Pedidos
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (pedido is null)
            {
                return new RetornoBaseContrato(false, "Pedido não encontrado.");
            }

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync(cancellationToken);
            return new RetornoBaseContrato(true, "Pedido excluido com sucesso.");
        }
        catch (Exception ex)
        {
            return new RetornoBaseContrato(false, ex.Message);
        }
    }
}
