using GestaoFinanceira.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Infra.Data.Mappings
{
    public class ItemMovimentacaoMap :IEntityTypeConfiguration<ItemMovimentacao>
    {
        public void Configure(EntityTypeBuilder<ItemMovimentacao> builder)
        {
            builder.ToTable("ITEM_MOVIMENTACAO");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Descricao)
                .HasColumnName("DESCRICAO_ITMO")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(i => i.Tipo)
                .HasColumnName("TIPO_ITMO")
                .HasMaxLength(1)
                .IsRequired();

            builder.Property(i => i.Status)
                .HasColumnName("STATUS_ITMO")
                .IsRequired();

            builder.Property(i => i.IdCategoria)
                .HasColumnName("ID_CATE")
                .IsRequired();

            builder.HasOne(i => i.Categoria)
                .WithMany(c => c.ItemMovimentacoes)
                .HasForeignKey(i => i.IdCategoria);
        }
    }
}
