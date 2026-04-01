using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Dominio.Modelos;

namespace Vendas.Infra.Database.Mapeamentos;

internal class PedidoMap : BaseMap<Pedido>
{
    public PedidoMap() : base("pedidos") { }

    public override void Configure(EntityTypeBuilder<Pedido> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Total).HasPrecision(18,4).HasColumnName("TOTAL").IsRequired();
        builder.Property(x => x.DataCriacao)
            .HasColumnName("DATA_CRIACAO")
            .HasColumnType("datetime2(3)")            
            .HasDefaultValueSql("GETUTCDATE()")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.HasMany(p => p.ItensPedido)
           .WithOne(ip => ip.Pedido)
           .HasForeignKey(ip => ip.PedidoId)
           .OnDelete(DeleteBehavior.Cascade)
           .IsRequired();
    }
}
