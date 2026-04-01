namespace Microsoft.Extensions.DependencyInjection;

public static class InstalarDependencias
{
    public static void AdicionarServicosApi(this IServiceCollection servicos, IConfiguration configuracao)
    {
        servicos.AdicionarMediadoresVendas(configuracao);
    }
}

