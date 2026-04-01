using Estoque.Dominio.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estoque.Infra.Database.Mapeamentos;

internal class MovimentoEstoqueMap : BaseMap<MovimentoEstoque>
{
    public MovimentoEstoqueMap() : base("movimento_estoque") { }

    public override void Configure(EntityTypeBuilder<MovimentoEstoque> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Tipo).HasColumnName("TIPO").IsRequired();
        builder.Property(x => x.DataCriacao)
            .HasColumnName("DATA_CRIACAO")
            .HasColumnType("datetime2(3)")             // precisão de milissegundos
            .HasDefaultValueSql("GETUTCDATE()")
            .ValueGeneratedOnAdd()
            .IsRequired();
        builder.Property(x => x.Quantidade).HasColumnName("QUANTIDADE").IsRequired(false);
        builder.Property(x => x.PedidoExternoId).HasColumnName("PEDIDO_EXTERNO_ID").IsRequired(false);

        builder.HasOne(x => x.ProdutoEstoque)
            .WithMany()
            .HasForeignKey(x => x.ProdutoEstoqueId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
