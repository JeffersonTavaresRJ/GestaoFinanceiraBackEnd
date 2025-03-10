using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Domain.Models.Enuns;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Infra.Data.Repositories.EntityFramework.Mappings
{
    public class MovimentacaoMap : IEntityTypeConfiguration<Movimentacao>
    {
        public void Configure(EntityTypeBuilder<Movimentacao> builder)
        {
            builder.ToTable("MOVIMENTACAO");

            //declarado no OnModelCreating do sqlContext, porque a classe GenericRepository está configurada
            //para que as consultas não sejam acompanhadas de leitura..
            builder.HasKey(m => new { m.IdItemMovimentacao, m.DataReferencia });

            builder.Property(m => m.IdItemMovimentacao)
                .HasColumnName("ID_ITMO")
                .IsRequired();

            builder.Property(m => m.DataReferencia)
                .HasColumnName("DATA_REFERENCIA_MOVI")
                .HasColumnType("DATE")
                .IsRequired();

            builder.Property(m => m.TipoPrioridade)
                .HasColumnName("TIPO_PRIORIDADE_MOVI")
                .HasMaxLength(1)
                .IsRequired()
                .HasConversion(v => v.ToString(),
                               v => (TipoPrioridade)Enum.Parse(typeof(TipoPrioridade), v));

            builder.HasOne(m => m.ItemMovimentacao)
                .WithMany(i => i.Movimentacoes)
                .HasForeignKey(m => m.IdItemMovimentacao);
        }
    }
}
