using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace GestionaleMuseoOfficial.Models
{
    public partial class ModelDbContext : DbContext
    {
        public ModelDbContext()
            : base("name=ModelDbContext")
        {
        }

        public virtual DbSet<Biglietti> Biglietti { get; set; }
        public virtual DbSet<CategorieVisitatori> CategorieVisitatori { get; set; }
        public virtual DbSet<Clienti> Clienti { get; set; }
        public virtual DbSet<Login> Login { get; set; }
        public virtual DbSet<Visite> Visite { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Biglietti>()
                .Property(e => e.PrezzoTotale)
                .HasPrecision(19, 4);

            modelBuilder.Entity<CategorieVisitatori>()
                .HasMany(e => e.Biglietti)
                .WithRequired(e => e.CategorieVisitatori)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Clienti>()
                .HasMany(e => e.Biglietti)
                .WithRequired(e => e.Clienti)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Visite>()
                .Property(e => e.TariffaOrdinaria)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Visite>()
                .HasMany(e => e.Biglietti)
                .WithRequired(e => e.Visite)
                .WillCascadeOnDelete(false);
        }
    }
}
