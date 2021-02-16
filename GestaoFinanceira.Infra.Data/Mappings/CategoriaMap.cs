using GestaoFinanceira.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoFinanceira.Infra.Data.Mappings
{
    public class CategoriaMap :IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("CATEGORIA");

            builder.HasKey(c => c.Id);               

            builder.Property(c => c.Descricao)
                .HasColumnName("DESCRICAO_CATE")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.Tipo)
                .HasColumnName("TIPO_CATE")
                .HasMaxLength(1)                
                .IsRequired();

            builder.Property(c => c.Status)
                .HasColumnName("STATUS_CATE")                
                .IsRequired();

            builder.Property(c => c.IdUsuario)
                .HasColumnName("ID_USUA")
                .IsRequired();

            builder.HasOne(c => c.Usuario)
                .WithMany(u => u.Categorias)
                .HasForeignKey(c => c.IdUsuario);
        }
    }
}
