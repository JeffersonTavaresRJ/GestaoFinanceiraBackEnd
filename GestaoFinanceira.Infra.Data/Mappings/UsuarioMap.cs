using GestaoFinanceira.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Infra.Data.Mappings
{
    public class UsuarioMap :IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("USUARIO");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasColumnName("ID");

            builder.Property(u => u.Nome)
                .HasColumnName("NOME")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(u => u.EMail)
                .HasColumnName("EMAIL")
                .HasMaxLength(50)
                .IsRequired();
                

            builder.Property(u => u.Senha)
                .HasColumnName("SENHA")
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
