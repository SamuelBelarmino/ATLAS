using Invest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Invest.Infrastructure.Data.Configurations;

public class OperacaoFinanceiraConfiguration : IEntityTypeConfiguration<OperacaoFinanceira>
{
    public void Configure(EntityTypeBuilder<OperacaoFinanceira> builder)
    {
        builder.ToTable("OperacoesFinanceiras");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Tipo)
            .IsRequired();

        builder.Property(o => o.Quantidade)
            .IsRequired();

        builder.Property(o => o.PrecoUnitario)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(o => o.DataOperacao)
            .IsRequired();

        builder.Property(o => o.DataCriacao)
            .IsRequired();
    }
}
