using GestaoFinanceira.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoFinanceira.Infra.Data.Repositories.EntityFramework.Mappings
{
    public class MovimentacaoRealizadaMap : IEntityTypeConfiguration<MovimentacaoRealizada>
    {
        public void Configure(EntityTypeBuilder<MovimentacaoRealizada> builder)
        {
            builder.ToTable("MOVIMENTACAO_REALIZADA");

            builder.HasKey(mr => mr.Id);

            builder.Property(mr => mr.IdItemMovimentacao)
                .HasColumnName("ID_ITMO")
                .IsRequired();

            builder.Property(mr => mr.IdConta)
                .HasColumnName("ID_CONT")
                .IsRequired();

            builder.Property(mr => mr.IdFormaPagamento)
                .HasColumnName("ID_FOPA")
                .IsRequired();

            builder.Property(mr => mr.IdMovimentacaoPrevista)
                .HasColumnName("ID_MOPR");

            builder.Property(mr => mr.DataReferencia)
                .HasColumnName("DATA_REFERENCIA_MOVI")
                .IsRequired();

            builder.Property(mr => mr.DataMovimentacaoRealizada)
                .HasColumnName("DATA_MORE")
                .IsRequired();

            builder.Property(mr => mr.Valor)
                .HasColumnName("VALOR_MORE")
                .IsRequired();

            builder.Property(mp => mp.Observacao)
               .HasColumnName("OBSERVACAO_MORE")
               .HasMaxLength(200);

            builder.HasOne(mr => mr.Conta)
                .WithMany(c => c.MovimentacoesRealizadas)
                .HasForeignKey(mr => mr.IdConta);

            builder.HasOne(mr => mr.FormaPagamento)
                .WithMany(f => f.MovimentacoesRealizadas)
                .HasForeignKey(mr => mr.IdFormaPagamento);

            builder.HasOne(mr => mr.MovimentacaoPrevista)
               .WithMany(mp => mp.MovimentacoesRealizadas)
               .HasForeignKey(mr => mr.IdMovimentacaoPrevista);

            builder.HasOne(mr => mr.Movimentacao)
                .WithMany(m => m.MovimentacoesRealizadas)
                .HasForeignKey(mr => new { mr.IdItemMovimentacao, mr.DataReferencia })
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
