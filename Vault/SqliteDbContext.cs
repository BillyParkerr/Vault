using Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Application
{
    public class SqliteDbContext : DbContext
    {
        public DbSet<EncryptedFile> EncryptedFiles { get; set; }

        public DbSet<EncryptionKey> EncryptionKeys { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("FileName=VaultDb", option =>
            {
                option.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EncryptedFile>().ToTable("EncryptedFile", "Vault");
            modelBuilder.Entity<EncryptedFile>(entity =>
            {
                entity.HasKey(k => k.Id);
                entity.HasIndex(i => i.FilePath).IsUnique();
                entity.HasIndex(i => i.UniquePassword);
            });

            modelBuilder.Entity<EncryptionKey>().ToTable("EncryptionKey", "Vault");
            modelBuilder.Entity<EncryptionKey>(entity =>
            {
                entity.HasKey(k => k.Id);
                entity.HasIndex(i => i.Key).IsUnique();
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
