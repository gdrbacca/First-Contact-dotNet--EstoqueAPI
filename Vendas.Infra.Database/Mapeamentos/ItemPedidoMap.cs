using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Dominio.Modelos;

namespace Vendas.Infra.Database.Mapeamentos;

internal class ItemPedidoMap : BaseMap<ItemPedido>
{
    public ItemPedidoMap() : base("item_pedido") { }

    public override void Configure(EntityTypeBuilder<ItemPedido> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Quantidade).HasColumnName("QUANTIDADE").IsRequired();
        builder.Property(x => x.ValorTotal).HasPrecision(18,4).HasColumnName("VALOR_TOTAL").IsRequired();
        builder.Property(x => x.ProdutoId).HasColumnName("PRODUTO_FK").IsRequired();
        builder.Property(x => x.PedidoId).HasColumnName("PEDIDO_FK").IsRequired();

        builder.HasOne(x => x.Produto)
            .WithMany()
            .HasForeignKey(x => x.ProdutoId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
