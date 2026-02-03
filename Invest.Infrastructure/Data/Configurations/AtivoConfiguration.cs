using Invest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Invest.Infrastructure.Data.Configurations;

public class AtivoConfiguration : IEntityTypeConfiguration<Ativo>
{
    public void Configure(EntityTypeBuilder<Ativo> builder)
    {
        builder.ToTable("Ativos");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Codigo)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(a => a.Codigo)
            .IsUnique();

        builder.Property(a => a.Tipo)
            .IsRequired();

        builder.Property(a => a.Mercado)
            .IsRequired();

        builder.HasMany(a => a.Operacoes)
            .WithOne(o => o.Ativo)
            .HasForeignKey(o => o.AtivoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
