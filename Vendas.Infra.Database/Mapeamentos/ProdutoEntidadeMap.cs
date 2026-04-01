using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Dominio.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Dominio.Modelos;

namespace Vendas.Infra.Database.Mapeamentos;

internal class ProdutoEntidadeMap : BaseMap<ProdutoEntidade>
{
    public ProdutoEntidadeMap() : base("produtos") { }

    public override void Configure(EntityTypeBuilder<ProdutoEntidade> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Nome).HasColumnName("NOME").IsRequired(true);
        builder.Property(x => x.Descricao).HasColumnName("DESCRICAO").IsRequired(true);
        builder.Property(x => x.ValorUnitario).HasPrecision(18,4).HasColumnName("VALOR_UNITARIO").IsRequired(true);
    }
}