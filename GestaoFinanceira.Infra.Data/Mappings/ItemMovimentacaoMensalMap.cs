using GestaoFinanceira.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoFinanceira.Infra.Data.Mappings
{
    public class ItemMovimentacaoMensalMap :IEntityTypeConfiguration<ItemMovimentacaoMensal>
    {
        public void Configure(EntityTypeBuilder<ItemMovimentacaoMensal> builder)
        {
            builder.ToView("VW_MOME_ITMO");

            builder.HasKey(s => new { s.IdUsuario, s.DataReferencia, s.IdCategoria, s.IdItemMovimentacao });

            builder.Property(s => s.IdUsuario)
                .HasColumnName("ID_USUA")
                .HasColumnType("INT");

            builder.Property(s => s.DataReferencia)
                .HasColumnName("DATA_REFERENCIA_MOVI")
                .HasColumnType("DATE");

            builder.Property(s => s.IdCategoria)
                .HasColumnName("ID_CATE")
                .HasColumnType("INT");

            builder.Property(s => s.DescricaoCategoria)
                .HasColumnName("DESCRICAO_CATE")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(50);

            builder.Property(s => s.IdItemMovimentacao)
                .HasColumnName("ID_ITMO")
                .HasColumnType("INT");

            builder.Property(s => s.DescricaoItemMovimentacao)
                .HasColumnName("DESCRICAO_ITMO")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(50);

            builder.Property(s => s.TipoItemMovimentacao)
                .HasColumnName("TIPO_ITMO")
                .HasColumnType("CHAR")
                .HasMaxLength(1);

            builder.Property(s => s.DescricaoTipoItemMovimentacao)
                .HasColumnName("DESCRICAO_TIPO_ITMO")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(10);

            builder.Property(s => s.TipoOperacao)
                .HasColumnName("TIPO_OPERACAO_ITMO")
                .HasColumnType("CHAR")
                .HasMaxLength(2);

            builder.Property(s => s.ValorMensal)
                .HasColumnName("VALOR_MENSAL_ITMO")
                .HasColumnType("FLOAT");

            builder.Property(s => s.DiferencaPercentualMensal)
                .HasColumnName("DIF_PERC_MENSAL_ITMO")
                .HasColumnType("FLOAT");            

        }
    }
}