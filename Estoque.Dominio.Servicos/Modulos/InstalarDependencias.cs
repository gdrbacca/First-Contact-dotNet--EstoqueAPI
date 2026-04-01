using Estoque.Dominio.Interfaces;
using Estoque.Dominio.Servicos.Servicos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class InstalarDependencias
{
    public static void AdicionarServicos(this IServiceCollection servicos, IConfiguration configuracao)
    {
        servicos.TryAddScoped<IProdutoEstoqueServico, ProdutoEstoqueServico>();
        servicos.TryAddScoped<IMovimentoEstoqueServico, MovimentoEstoqueServico>();
    }
}
