using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Dominio.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Dominio.Interfaces;
using Vendas.Dominio.Modelos;

namespace Vendas.Infra.Database.Contextos;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public override int SaveChanges()
    {
        AjustarTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AjustarTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void AjustarTimestamps()
    {
        var agoraUtc = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries())
        {
            // DataCriacao só na inserção
            if (entry.Entity is IDataCriacao criacao && entry.State == EntityState.Added)
            {
                criacao.DataCriacao = agoraUtc;
            }

            // UltimaAtualizacao -> na inserção e na atualização
            if (entry.Entity is IUltimaAtualizacao atualizacao &&
                (entry.State == EntityState.Added || entry.State == EntityState.Modified))
            {
                atualizacao.UltimaAtualizacao = agoraUtc;

                // Protege o DataCriacao (se a entidade também tiver)
                if (entry.Entity is IDataCriacao)
                {
                    entry.Property(nameof(IDataCriacao.DataCriacao)).IsModified = false;
                }
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    public DbSet<ProdutoEntidade> Produtos { get; set; }
    public DbSet<ItemPedido> ItensPedido { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
}
