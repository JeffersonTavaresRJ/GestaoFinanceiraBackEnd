using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Domain.Models.Enuns;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Infra.Data.Mappings
{
    public class MovimentacaoPrevistaMap : IEntityTypeConfiguration<MovimentacaoPrevista>
    {
        public void Configure(EntityTypeBuilder<MovimentacaoPrevista> builder)
        {
            builder.ToTable("MOVIMENTACAO_PREVISTA");

            builder.HasKey(mp => new { mp.IdItemMovimentacao, mp.DataReferencia });

            builder.Property(mp => mp.IdItemMovimentacao)
                .HasColumnName("ID_ITMO")
                .IsRequired();

            builder.Property(mp => mp.DataReferencia)
                .HasColumnName("DATA_REFERENCIA_MOVI")
                .HasColumnType("DATE")
                .IsRequired();

            builder.Property(mp => mp.DataVencimento)
               .HasColumnName("DATA_VENCIMENTO_MOPR")
               .HasColumnType("DATE")
               .IsRequired();

            builder.Property(mp => mp.Valor)
               .HasColumnName("VALOR_MOPR")
               .IsRequired();

            builder.Property(mp => mp.Status)
               .HasColumnName("STATUS_MOPR")
               .HasMaxLength(1)
               .IsRequired()
               .HasConversion(v => v.ToString(),
                              v => (StatusMovimentacaoPrevista)Enum.Parse(typeof(StatusMovimentacaoPrevista), v));

            builder.Property(mp => mp.IdFormaPagamento)
               .HasColumnName("ID_FOPA")
               .IsRequired();

            builder.Property(mp => mp.NrParcela)
               .HasColumnName("NR_PARCELA")
               .IsRequired();

            builder.Property(mp => mp.NrParcelaTotal)
               .HasColumnName("NR_PARCELA_TOTAL")
               .IsRequired();

            builder.HasOne(mp => mp.FormaPagamento)
               .WithMany(m => m.MovimentacoesPrevistas)
               .HasForeignKey(mp => mp.IdFormaPagamento);

            builder.HasOne(mp => mp.Movimentacao)
                .WithOne(m => m.MovimentacaoPrevista)                
                .HasForeignKey<MovimentacaoPrevista>(mp => new { mp.IdItemMovimentacao, mp.DataReferencia })
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}