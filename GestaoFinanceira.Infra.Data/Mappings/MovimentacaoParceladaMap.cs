using GestaoFinanceira.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Infra.Data.Mappings
{
    public class MovimentacaoParceladaMap : IEntityTypeConfiguration<MovimentacaoParcelada>
    {
        public void Configure(EntityTypeBuilder<MovimentacaoParcelada> builder)
        {
            builder.ToTable("MOVIMENTACAO_PARCELADA");

            builder.HasKey(mp => new { mp.IdItemMovimentacao, mp.DataReferencia, mp.ParcelaAtual});

            builder.Property(mp => mp.IdItemMovimentacao)
                .HasColumnName("ID_ITMO")
                .IsRequired();

            builder.Property(mp => mp.DataReferencia)
                .HasColumnName("DATA_REFERENCIA_MOVI")
                .HasColumnType("DATE")
                .IsRequired();

            builder.Property(mp => mp.ParcelaAtual)
                .HasColumnName("PARCELA_ATUAL_MOPA")
                .IsRequired();

            builder.Property(mp => mp.ParcelaTotal)
                .HasColumnName("PARCELA_TOTAL_MOPA")
                .IsRequired();

            builder.HasOne(mp => mp.Movimentacao)
                .WithMany(m => m.MovimentacoesParceladas)
                .HasForeignKey(mp => new { mp.IdItemMovimentacao, mp.DataReferencia });
        }
    }
}
