using Microsoft.Extensions.Logging;
using Shared.Dominio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Vendas.Dominio.Interfaces;

namespace Vendas.Infra.HTTP.Servicos;

public class EstoqueHTTPClient : IEstoqueServicoHTTP
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<EstoqueHTTPClient> _logger;

    public EstoqueHTTPClient(
            HttpClient httpClient,          // ← injetado pelo factory
            ILogger<EstoqueHTTPClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<ProdutoEstoqueQuantidadeContrato> ObterQuantidadeEstoqueProdutoAsync(Guid produtoId, CancellationToken cancellation = default)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ProdutoEstoqueQuantidadeContrato>(
                $"api/Estoque/quantidade/{produtoId}", cancellation);

            return response ?? throw new Exception("Estoque não encontrado");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Falha ao consultar estoque do produto {ProdutoId}", produtoId);
            throw; // ou return Result.Failure(...)
        }
    }      
}
