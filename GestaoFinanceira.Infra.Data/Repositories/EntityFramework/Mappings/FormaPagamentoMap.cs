using GestaoFinanceira.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Infra.Data.Repositories.EntityFramework.Mappings
{
    public class FormaPagamentoMap : IEntityTypeConfiguration<FormaPagamento>
    {
        public void Configure(EntityTypeBuilder<FormaPagamento> builder)
        {
            builder.ToTable("FORMA_PAGAMENTO");

            builder.HasKey(f => f.Id);

            builder.Property(f => f.Descricao)
                .HasColumnName("DESCRICAO_FOPA")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(f => f.Status)
                .HasColumnName("STATUS_FOPA")
                .IsRequired();

            builder.Property(f => f.IdUsuario)
                .HasColumnName("ID_USUA")
                .IsRequired();

            builder.HasOne(u => u.Usuario)
                .WithMany(f => f.FormasPagamento)
                .HasForeignKey(f => f.IdUsuario);

        }
    }
}
