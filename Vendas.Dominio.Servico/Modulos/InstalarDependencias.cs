using Vendas.Dominio.Interfaces;
using Vendas.Dominio.Servico.Servicos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class InstalarDependencias
{
    public static void AdicionarServicos(this IServiceCollection servicos, IConfiguration configuracoes)
    {
        servicos.TryAddScoped<IProdutoServico, ProdutoServico>();
        servicos.TryAddScoped<IPedidoServico, PedidoServico>();
    }
}
