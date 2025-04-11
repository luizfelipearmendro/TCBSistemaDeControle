using Microsoft.EntityFrameworkCore;
using TCBSistemaDeControle.Models;

namespace TCBSistemaDeControle.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<UsuariosModel> Usuarios { get; set; }

        public DbSet<FuncionariosModel> Funcionarios { get; set; }

        public DbSet<RacaModel> Raca { get; set; }

        public DbSet<EstadoCivilModel> EstadoCivil { get; set; }

        public DbSet<SetoresModel> Setores { get; set; }

        public DbSet<CategoriaModel> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da entidade SetoresModel
            modelBuilder.Entity<SetoresModel>(entity =>
            {
                // Chave primária
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Id).UseMySqlIdentityColumn();

                // Nome do setor
                entity.Property(s => s.Nome)
                      .HasColumnType("varchar(100)") // Tamanho máximo de 100 caracteres
                      .IsRequired(); // Obrigatório

                // Descrição do setor
                entity.Property(s => s.Descricao)
                      .HasColumnType("varchar(255)"); // Tamanho máximo de 255 caracteres

                // Imagem (array de bytes)
                entity.Property(s => s.ImagemSetor)
                       .HasColumnType("varbinary(max)") // ✅ Correto para SQL Server
                       .IsRequired(false); // Não obrigatório (nullable)

                // Outras propriedades...
                entity.Property(s => s.ResponsavelSetor)
                      .HasColumnType("varchar(100)")
                      .IsRequired();

                entity.Property(s => s.EmailResposavelSetor)
                      .HasColumnType("varchar(100)")
                      .IsRequired();

                entity.Property(s => s.Localizacao)
                      .HasColumnType("varchar(255)");

                entity.Property(s => s.DataCriacao)
                      .HasColumnType("datetime")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP")
                      .ValueGeneratedOnAdd()
                      .IsRequired();

                entity.Property(s => s.DataAtualizacao)
                      .HasColumnType("datetime")
                      .ValueGeneratedOnAddOrUpdate()
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(s => s.Ativo)
                      .HasColumnType("char(1)")
                      .IsRequired();

                entity.Property(s => s.UsuarioId)
                      .HasColumnType("int")
                      .IsRequired();

                entity.Property(s => s.CategoriaId)
                      .HasColumnType("int")
                      .IsRequired();
            });
        }
    }
}
