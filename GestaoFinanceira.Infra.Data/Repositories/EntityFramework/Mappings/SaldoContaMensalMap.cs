using GestaoFinanceira.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoFinanceira.Infra.Data.Repositories.EntityFramework.Mappings
{
    public class SaldoContaMensalMap : IEntityTypeConfiguration<SaldoContaMensal>
    {
        public void Configure(EntityTypeBuilder<SaldoContaMensal> builder)
        {
            builder.ToView("VW_SAME_CONT");

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

            builder.Property(s => s.DezembroAnterior)
                .HasColumnName("DEZEMBRO_ANT")
                .HasColumnType("FLOAT");

            builder.Property(s => s.Janeiro)
                .HasColumnName("JANEIRO")
                .HasColumnType("FLOAT");

            builder.Property(s => s.PercJaneiro)
                .HasColumnName("PERC_JAN")
                .HasColumnType("FLOAT");

            builder.Property(s => s.Fevereiro)
                .HasColumnName("FEVEREIRO")
                .HasColumnType("FLOAT");

            builder.Property(s => s.PercFevereiro)
                .HasColumnName("PERC_FEV")
                .HasColumnType("FLOAT");

            builder.Property(s => s.Marco)
                .HasColumnName("MARCO")
                .HasColumnType("FLOAT");

            builder.Property(s => s.PercMarco)
                .HasColumnName("PERC_MAR")
                .HasColumnType("FLOAT");

            builder.Property(s => s.Abril)
                .HasColumnName("ABRIL")
                .HasColumnType("FLOAT");

            builder.Property(s => s.PercAbril)
                .HasColumnName("PERC_ABR")
                .HasColumnType("FLOAT");

            builder.Property(s => s.Maio)
                .HasColumnName("MAIO")
                .HasColumnType("FLOAT");

            builder.Property(s => s.PercMaio)
                .HasColumnName("PERC_MAI")
                .HasColumnType("FLOAT");

            builder.Property(s => s.Junho)
                .HasColumnName("JUNHO")
                .HasColumnType("FLOAT");

            builder.Property(s => s.PercJunho)
                .HasColumnName("PERC_JUN")
                .HasColumnType("FLOAT");

            builder.Property(s => s.Julho)
                .HasColumnName("JULHO")
                .HasColumnType("FLOAT");

            builder.Property(s => s.PercJulho)
                .HasColumnName("PERC_JUL")
                .HasColumnType("FLOAT");

            builder.Property(s => s.Agosto)
                .HasColumnName("AGOSTO")
                .HasColumnType("FLOAT");

            builder.Property(s => s.PercAgosto)
                .HasColumnName("PERC_AGO")
                .HasColumnType("FLOAT");

            builder.Property(s => s.Setembro)
                .HasColumnName("SETEMBRO")
                .HasColumnType("FLOAT");

            builder.Property(s => s.PercSetembro)
                .HasColumnName("PERC_SET")
                .HasColumnType("FLOAT");

            builder.Property(s => s.Outubro)
                .HasColumnName("OUTUBRO")
                .HasColumnType("FLOAT");

            builder.Property(s => s.PercOutubro)
                .HasColumnName("PERC_OUT")
                .HasColumnType("FLOAT");

            builder.Property(s => s.Novembro)
                .HasColumnName("NOVEMBRO")
                .HasColumnType("FLOAT");

            builder.Property(s => s.PercNovembro)
                .HasColumnName("PERC_NOV")
                .HasColumnType("FLOAT");

            builder.Property(s => s.Dezembro)
                .HasColumnName("DEZEMBRO")
                .HasColumnType("FLOAT");

            builder.Property(s => s.PercDezembro)
                .HasColumnName("PERC_DEZ")
                .HasColumnType("FLOAT");

        }
    }
}