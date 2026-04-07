namespace Vendas.Dominio.Contratos;

public record RetornoBaseContrato(
    bool Sucesso,
    string MensagemErro,
    Guid? Id = null
);
