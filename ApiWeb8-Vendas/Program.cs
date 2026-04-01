using MassTransit;
using Mensageria.Contratos.Contratos;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using Vendas.Dominio.Interfaces;
using Vendas.Infra.Database.Contextos;
using Vendas.Infra.HTTP.Servicos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AdicionarServicosApi(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// adicionando http client que vai chamar o estoque
builder.Services.AddHttpClient("EstoqueApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:EstoqueApi"]);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddScoped<IEstoqueServicoHTTP>(sp =>
{
    var factory = sp.GetRequiredService<IHttpClientFactory>();
    var client = factory.CreateClient("EstoqueApi");
    var logger = sp.GetRequiredService<ILogger<EstoqueHTTPClient>>();
    return new EstoqueHTTPClient(client, logger);
});
//////////

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();


    x.AddEntityFrameworkOutbox<AppDbContext>(o =>
        {
            o.UseSqlServer();
            o.UseBusOutbox();
            o.QueryDelay = TimeSpan.FromSeconds(5);
        }
    );

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.Publish<PedidoCriadoEvent>(p =>
        {
            p.ExchangeType = ExchangeType.Fanout;
            // p.Durable = true;  // default já é true
        });

        cfg.ConfigureEndpoints(context);
    });
});

// builder.Services.AddMassTransitHostedService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
