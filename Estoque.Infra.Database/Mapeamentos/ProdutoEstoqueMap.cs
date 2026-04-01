using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Dominio.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estoque.Infra.Database.Mapeamentos;

internal class ProdutoEstoqueMap : BaseMap<ProdutoEstoque>
{
    public ProdutoEstoqueMap() : base("produto_estoque") { }

    public override void Configure(EntityTypeBuilder<ProdutoEstoque> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.QuantidadeDisponivel).HasColumnName("QUANTIDADE_DISPONIVEL").IsRequired();
        builder.Property(x => x.EstoqueMinimo).HasColumnName("ESTOQUE_MINIMO").IsRequired();
        builder.Property(x => x.UltimaAtualizacao).HasColumnName("ULTIMA_ATUALIZACAO").HasColumnType("datetime2(3)").IsRequired();

        builder.HasOne<ProdutoEntidade>() //HasOne(typeof(ProdutoEntidade))
            .WithMany()
            .HasForeignKey(p => p.ProdutoId) //"ProdutoId"
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
