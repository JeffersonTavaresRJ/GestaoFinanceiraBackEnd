using GestaoFinanceira.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Infra.Data.Mappings
{
    public class ContaMap : IEntityTypeConfiguration<Conta>
    {
        public void Configure(EntityTypeBuilder<Conta> builder)
        {
            builder.ToTable("CONTA");
            
            builder.HasKey(c => c.Id);
            
            builder.Property(c=>c.Descricao)
                .HasColumnName("DESCRICAO_CONT")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.Status)
                .HasColumnName("STATUS_CONT")
                .IsRequired();

            builder.Property(c => c.IdUsuario)
                .HasColumnName("ID_USUA")
                .IsRequired();

            builder.HasOne(c => c.Usuario)
                .WithMany(u => u.Contas)
                .HasForeignKey(c => c.IdUsuario);

        }
    }
}
