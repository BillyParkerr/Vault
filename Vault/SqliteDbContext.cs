﻿using Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Application;

public class SqliteDbContext : DbContext
{
    public DbSet<EncryptedFile> EncryptedFiles { get; set; }
    public DbSet<EncryptionKey> EncryptionKeys { get; set; }
    private string DbPath { get; }

    public SqliteDbContext()
    {
        var programDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "PersonalVaultApplication");
        DbPath = Path.Join(programDirectory, "VaultDb.db");

        Database.EnsureCreated();
    }

    public SqliteDbContext(DbContextOptions<SqliteDbContext> options) : base(options)
    {
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
        {
            options.UseSqlite($"Data Source={DbPath}");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EncryptedFile>().ToTable("EncryptedFile", "Vault");
        modelBuilder.Entity<EncryptedFile>(entity =>
        {
            entity.HasKey(k => k.Id);
            entity.Property(p => p.FilePath).IsRequired();
            entity.HasIndex(i => i.FilePath).IsUnique();
        });

        modelBuilder.Entity<EncryptionKey>().ToTable("EncryptionKey", "Vault");
        modelBuilder.Entity<EncryptionKey>(entity =>
        {
            entity.HasKey(k => k.Id);
            entity.Property(p => p.Key).IsRequired();
            entity.HasIndex(i => i.Key).IsUnique();
        });
        base.OnModelCreating(modelBuilder);
    }
}