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
        public DbSet<Conta> Contas { get; set; }
        public DbSet<FormaPagamento> FormasPagamento { get; set; }
        public DbSet<ItemMovimentacao> ItensMovimentacao { get; set; }
        public DbSet<Movimentacao> Movimentacoes { get; set; }
        public DbSet<MovimentacaoPrevista> MovimentacoesPrevistas { get; set; }
        public DbSet<MovimentacaoRealizada> MovimentacoesRealizadas { get; set; }


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

            modelBuilder.ApplyConfiguration(new ContaMap());
            modelBuilder.Entity<Conta>(entity => entity.Property(c => c.Id).ValueGeneratedOnAdd());

            modelBuilder.ApplyConfiguration(new FormaPagamentoMap());
            modelBuilder.Entity<FormaPagamento>(entity => entity.Property(c => c.Id).ValueGeneratedOnAdd());

            modelBuilder.ApplyConfiguration(new ItemMovimentacaoMap());
            modelBuilder.Entity<ItemMovimentacao>(entity => entity.Property(c => c.Id).ValueGeneratedOnAdd());            
            modelBuilder.Entity<ItemMovimentacao>()
                        .Property(c => c.Tipo)
                        .HasConversion(
                                        v => v.ToString(),
                                        v => (TipoItemMovimentacao)Enum.Parse(typeof(TipoItemMovimentacao), v));

            modelBuilder.ApplyConfiguration(new MovimentacaoMap());
            modelBuilder.Entity<Movimentacao>(entity => entity.HasKey(m => new { m.IdItemMovimentacao, m.DataReferencia}));
            modelBuilder.Entity<Movimentacao>()
                        .Property(m => m.TipoPrioridade)
                        .HasConversion(
                                        v => v.ToString(),
                                        v => (TipoPrioridade)Enum.Parse(typeof(TipoPrioridade), v));
            /*relacionamento um para um..*/
            modelBuilder.Entity<Movimentacao>()
                .HasOne(m => m.MovimentacaoPrevista)
                .WithOne(mp => mp.Movimentacao)
                .HasForeignKey<MovimentacaoPrevista>(mp => new { mp.IdItemMovimentacao, mp.DataReferencia });  
            
            

            modelBuilder.ApplyConfiguration(new MovimentacaoPrevistaMap());
            modelBuilder.Entity<MovimentacaoPrevista>(entity => entity.HasKey(mp => new { mp.IdItemMovimentacao, mp.DataReferencia }));
            modelBuilder.Entity<MovimentacaoPrevista>()
                        .Property(mp => mp.Status)
                        .HasConversion(
                                        v => v.ToString(),
                                        v => (StatusMovimentacaoPrevista)Enum.Parse(typeof(StatusMovimentacaoPrevista), v));

            modelBuilder.ApplyConfiguration(new MovimentacaoRealizadaMap());
            modelBuilder.Entity<MovimentacaoRealizada>(entity => entity.Property(c => c.Id).ValueGeneratedOnAdd());



            base.OnModelCreating(modelBuilder);
        }
    }
}