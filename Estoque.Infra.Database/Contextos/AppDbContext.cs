using Shared.Dominio.Modelos;
using Estoque.Dominio.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estoque.Dominio.Interfaces;

namespace Estoque.Infra.Database.Contextos;

public class AppDbContextEstoque : DbContext
{
    public AppDbContextEstoque(DbContextOptions<AppDbContextEstoque> options) : base(options)
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
        var agoraUtc = DateTime.UtcNow;   // ← sempre use UTC

        foreach (var entry in ChangeTracker.Entries())
        {
            // DataCriacao → só na inserção
            if (entry.Entity is IDataCriacao criacao && entry.State == EntityState.Added)
            {
                criacao.DataCriacao = agoraUtc;
            }

            // UltimaAtualizacao → na inserção E na atualização
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

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        // modelBuilder.Ignore<ProdutoEntidade>();
    }

    public DbSet<ProdutoEstoque> ProdutoEstoque { get; set; }
    public DbSet<MovimentoEstoque> MovimentoEstoque { get; set; }
    // public DbSet<ProdutoEntidade> Produtos { get; set; }
}
