using Microsoft.EntityFrameworkCore;
using NoticiasApp.Models;

namespace NoticiasApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tag> Tags { get; set; }
        public DbSet<NoticiaTag> NoticiaTags { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Noticia> Noticias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da relação entre Tag e NoticiaTag
            modelBuilder.Entity<Tag>()
                .HasMany(t => t.NoticiaTags)
                .WithOne(nt => nt.Tag)
                .HasForeignKey(nt => nt.TagId);

            // Configuração da relação entre Noticia e NoticiaTag
            modelBuilder.Entity<Noticia>()
                .HasMany(n => n.NoticiaTags)
                .WithOne(nt => nt.Noticia)
                .HasForeignKey(nt => nt.NoticiaId);

            // Configuração da relação entre Noticia e Usuario
            modelBuilder.Entity<Noticia>()
                .HasOne(n => n.Usuario)
                .WithMany(u => u.Noticias)
                .HasForeignKey(n => n.UsuarioId);

            // Configuração da chave primária para NoticiaTag
            modelBuilder.Entity<NoticiaTag>()
                .HasKey(nt => nt.Id);

            // Configuração para que o Id seja gerado automaticamente
            modelBuilder.Entity<NoticiaTag>()
                .Property(nt => nt.Id)
                .ValueGeneratedOnAdd();

            // Configuração de campos e tamanhos
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Nome)
                .HasMaxLength(250);

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Senha)
                .HasMaxLength(50);

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Email)
                .HasMaxLength(250);

            modelBuilder.Entity<Noticia>()
                .Property(n => n.Titulo)
                .HasMaxLength(250);
        }


    }
}
