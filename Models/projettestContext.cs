using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace projett.Models
{
    public partial class projettestContext : DbContext
    {
        public projettestContext()
        {
        }

        public projettestContext(DbContextOptions<projettestContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Commande> Commandes { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Commande>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PK__commande__465962296A70EE6E");

                entity.ToTable("commandes");

                entity.Property(e => e.OrderId)
                    .ValueGeneratedNever()
                    .HasColumnName("order_id");

                entity.Property(e => e.Article)
                    .HasMaxLength(100)
                    .HasColumnName("article");

                entity.Property(e => e.Prix)
                    .HasMaxLength(100)
                    .HasColumnName("prix");

                entity.Property(e => e.Qte).HasMaxLength(100);

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Commandes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__commandes__user___398D8EEE");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("user_id");

                entity.Property(e => e.MotsDePasse)
                    .HasMaxLength(100)
                    .HasColumnName("mots_de_passe");

                entity.Property(e => e.Nom)
                    .HasMaxLength(100)
                    .HasColumnName("nom");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
