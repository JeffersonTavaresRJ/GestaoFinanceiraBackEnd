using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Domain.Models.Enuns;
using GestaoFinanceira.Infra.Data.Mappings;
using Microsoft.EntityFrameworkCore;
using System;

namespace GestaoFinanceira.Infra.Data.Context
{

    public class SqlContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        public SqlContext(DbContextOptions<SqlContext> options) : base(options)
        {

        }

        //TODO: LOG DE EXECUÇÃO DO EF CORE - PASSO 01: ALTERAR O MÉTODO OnConfiguring DO DBCONTEXT..
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.EnableSensitiveDataLogging(false);
     

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.Entity<Usuario>(entity => entity.HasIndex(u => u.EMail));

            modelBuilder.ApplyConfiguration(new CategoriaMap());
            modelBuilder.Entity<Categoria>(entity => entity.Property(c => c.Id).ValueGeneratedOnAdd());

            modelBuilder.Entity<Categoria>()
                        .Property(c => c.Tipo)
                        .HasConversion(
                                        v => v.ToString(),
                                        v => (TipoCategoria)Enum.Parse(typeof(TipoCategoria), v));


            base.OnModelCreating(modelBuilder);
        }
    }
}
