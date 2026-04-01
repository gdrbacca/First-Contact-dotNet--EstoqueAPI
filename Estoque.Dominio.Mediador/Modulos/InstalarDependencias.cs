using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class InstalarDependencias
{
    public static void AdicionarMediadoresEstoque(this IServiceCollection servicos, IConfiguration configuracao)
    {
        servicos.AdicionarServicos(configuracao);
    }
}
