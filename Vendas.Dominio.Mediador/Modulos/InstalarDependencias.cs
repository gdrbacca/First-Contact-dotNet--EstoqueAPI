using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class InstalarDependencias
{
    public static void AdicionarMediadoresVendas(this IServiceCollection servicos, IConfiguration configuracoes)
    {
        servicos.AdicionarServicos(configuracoes);
    }
}
