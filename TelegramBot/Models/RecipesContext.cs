using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace TelegramBot.Models
{
    public partial class RecipesContext : DbContext
    {
        public RecipesContext()
        {
        }

        public RecipesContext(DbContextOptions<RecipesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Ingredient> Ingredients { get; set; }
        public virtual DbSet<Recipe> Recipes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=C:\USERS\XORBYTEN\SOURCE\REPOS\TELEGRAMBOT\TELEGRAMBOT\DATABASE\RECIPES.MDF;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.Property(e => e.IngredientId).ValueGeneratedOnAdd();

                entity.Property(e => e.Ingr1)
                    .HasColumnType("nvarchar")
                    .HasColumnName("ingr1");

                entity.Property(e => e.Ingr10)
                    .HasColumnType("nvarchar")
                    .HasColumnName("ingr10");

                entity.Property(e => e.Ingr2)
                    .HasColumnType("nvarchar")
                    .HasColumnName("ingr2");

                entity.Property(e => e.Ingr3)
                    .HasColumnType("nvarchar")
                    .HasColumnName("ingr3");

                entity.Property(e => e.Ingr4)
                    .HasColumnType("nvarchar")
                    .HasColumnName("ingr4");

                entity.Property(e => e.Ingr5)
                    .HasColumnType("nvarchar")
                    .HasColumnName("ingr5");

                entity.Property(e => e.Ingr6)
                    .HasColumnType("nvarchar")
                    .HasColumnName("ingr6");

                entity.Property(e => e.Ingr7)
                    .HasColumnType("nvarchar")
                    .HasColumnName("ingr7");

                entity.Property(e => e.Ingr8)
                    .HasColumnType("nvarchar")
                    .HasColumnName("ingr8");

                entity.Property(e => e.Ingr9)
                    .HasColumnType("nvarchar")
                    .HasColumnName("ingr9");

                entity.HasOne(d => d.IngredientNavigation)
                    .WithOne(p => p.Ingredient)
                    .HasForeignKey<Ingredient>(d => d.IngredientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Ingredien__Ingre__25869641");
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("nvarchar");

                entity.Property(e => e.RecipeName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
