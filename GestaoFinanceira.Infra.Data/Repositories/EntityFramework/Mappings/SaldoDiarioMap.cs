using GestaoFinanceira.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Infra.Data.Repositories.EntityFramework.Mappings
{
    public class SaldoDiarioMap : IEntityTypeConfiguration<SaldoDiario>
    {
        public void Configure(EntityTypeBuilder<SaldoDiario> builder)
        {
            builder.ToTable("SALDO_DIARIO");

            builder.HasKey(sa => new { sa.IdConta, sa.DataSaldo });

            builder.Property(sa => sa.IdConta)
                .HasColumnName("ID_CONT")
                .IsRequired();

            builder.Property(sa => sa.DataSaldo)
                .HasColumnName("DATA_SADI")
                .HasColumnType("DATE")
                .IsRequired();

            builder.Property(sa => sa.Valor)
                .HasColumnName("VALOR_SADI")
                .IsRequired();

            builder.Property(sa => sa.Status)
                .HasColumnName("STATUS_SADI")
                .IsRequired();

            builder.HasOne(sa => sa.Conta)
                .WithMany(c => c.SaldosDiario)
                .HasForeignKey(sa => sa.IdConta);
        }
    }
}
