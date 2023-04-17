using GestaoFinanceira.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoFinanceira.Infra.Data.Mappings
{
    public class SaldoAnualMap :IEntityTypeConfiguration<SaldoAnual>
    {
        public void Configure(EntityTypeBuilder<SaldoAnual> builder)
        {
            builder.ToView("VW_SALDO_ANUAL");

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

            builder.Property(s => s.Janeiro)
                .HasColumnName("JANEIRO")
                .HasColumnType("FLOAT"); 

            builder.Property(s => s.Fevereiro)
                .HasColumnName("FEVEREIRO")
                .HasColumnType("FLOAT"); 

            builder.Property(s => s.Marco)
                .HasColumnName("MARCO")
                .HasColumnType("FLOAT");

            builder.Property(s => s.Abril)
                .HasColumnName("ABRIL")
                .HasColumnType("FLOAT"); 

            builder.Property(s => s.Maio)
                .HasColumnName("MAIO")
                .HasColumnType("FLOAT");

            builder.Property(s => s.Junho)
                .HasColumnName("JUNHO")
                .HasColumnType("FLOAT");

            builder.Property(s => s.Julho)
                .HasColumnName("JULHO")
                .HasColumnType("FLOAT");

            builder.Property(s => s.Agosto)
                .HasColumnName("AGOSTO")
                .HasColumnType("FLOAT");

            builder.Property(s => s.Setembro)
                .HasColumnName("SETEMBRO")
                .HasColumnType("FLOAT");

            builder.Property(s => s.Outubro)
                .HasColumnName("OUTUBRO")
                .HasColumnType("FLOAT");

            builder.Property(s => s.Novembro)
                .HasColumnName("NOVEMBRO")
                .HasColumnType("FLOAT");

            builder.Property(s => s.Dezembro)
                .HasColumnName("DEZEMBRO")
                .HasColumnType("FLOAT");

        }
    }
}