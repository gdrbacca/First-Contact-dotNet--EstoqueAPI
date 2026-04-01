using Estoque.Infra.Database.Mapeamentos;
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

//Estoque trata ProdutoEntidade como readOnly
internal class ProdutoEntidadeMap : BaseMap<ProdutoEntidade>
{
    public ProdutoEntidadeMap() : base("produtos") { }

    public override void Configure(EntityTypeBuilder<ProdutoEntidade> builder)
    {
        base.Configure(builder);


        foreach (var property in builder.Metadata.GetProperties())
        {
            property.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            // Opcional: property.SetBeforeSaveBehavior(PropertySaveBehavior.Throw);
        }
    }
}
