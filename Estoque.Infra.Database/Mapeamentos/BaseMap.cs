using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Dominio.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estoque.Infra.Database.Mapeamentos;

public class BaseMap<T> : IEntityTypeConfiguration<T> where T : EntidadeBase
{
    private readonly string _tableName;

    public BaseMap(string tableName)
    {
        _tableName = tableName;
    }

    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        if (!string.IsNullOrEmpty(_tableName)) builder.ToTable(_tableName);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
    }
}

