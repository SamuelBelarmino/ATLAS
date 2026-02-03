using Invest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Invest.Infrastructure.Data.Configurations;

public class CarteiraConfiguration : IEntityTypeConfiguration<Carteira>
{
    public void Configure(EntityTypeBuilder<Carteira> builder)
    {
        builder.ToTable("Carteiras");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.PerfilRisco)
            .IsRequired();

        builder.Property(c => c.DataCriacao)
            .IsRequired();

        builder.HasMany(c => c.Operacoes)
            .WithOne(o => o.Carteira)
            .HasForeignKey(o => o.CarteiraId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
