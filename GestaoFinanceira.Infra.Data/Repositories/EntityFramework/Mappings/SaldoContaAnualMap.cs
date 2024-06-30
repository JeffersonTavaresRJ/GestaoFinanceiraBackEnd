using GestaoFinanceira.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoFinanceira.Infra.Data.Repositories.EntityFramework.Mappings
{
    public class SaldoContaAnualMap : IEntityTypeConfiguration<SaldoContaAnual>
    {
        public void Configure(EntityTypeBuilder<SaldoContaAnual> builder)
        {
            builder.ToView("VW_SAAN_CONT");

            builder.HasKey(s => new { s.IdUsuario, s.IdConta, s.Ano });
            //builder.HasNoKey();

            builder.Property(s => s.IdUsuario)
                .HasColumnName("ID_USUA")
                .HasColumnType("INT");

            builder.Property(s => s.IdConta)
                .HasColumnName("ID_CONT")
                .HasColumnType("INT");

            builder.Property(s => s.DescricaoConta)
                .HasColumnName("DESCRICAO_CONT")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(50);

            builder.Property(s => s.Ano)
                .HasColumnName("ANO")
                .HasColumnType("INT");

            builder.Property(s => s.Saldo)
                .HasColumnName("SALDO")
                .HasColumnType("FLOAT");

            builder.Property(s => s.ValorInicial)
                .HasColumnName("VALOR_INICIAL")
                .HasColumnType("FLOAT");

            builder.Property(s => s.ReceitaTotalAnual)
                .HasColumnName("RECEITA_TOTAL_ANUAL")
                .HasColumnType("FLOAT");

            builder.Property(s => s.ReceitaMediaMensal)
                .HasColumnName("RECEITA_MEDIA_MENSAL")
                .HasColumnType("FLOAT");

            builder.Property(s => s.SaldoEsperado)
                .HasColumnName("SALDO_ESPERADO")
                .HasColumnType("FLOAT");
        }
    }
}