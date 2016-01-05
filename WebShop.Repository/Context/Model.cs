namespace WebShop.Repository
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model : DbContext
    {
        public Model()
            : base("name=shop")
        {
        }

        public virtual DbSet<Converter> Converter { get; set; }
        public virtual DbSet<Good> Good { get; set; }
        public virtual DbSet<Manufacturer> Manufacturer { get; set; }
        public virtual DbSet<Number> Number { get; set; }
        public virtual DbSet<Sale> Sale { get; set; }
        public virtual DbSet<SalePos> SalePos { get; set; }
        public virtual DbSet<Tax> Tax { get; set; }
        public virtual DbSet<PhotoGood> PhotoGoods { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Converter>()
                .Property(e => e.OneUsd)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Converter>()
                .Property(e => e.Hryvnia)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Good>()
                .Property(e => e.GoodName)
                .IsUnicode(false);

            modelBuilder.Entity<Good>()
                .Property(e => e.PriceUsd)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Good>()
                .Property(e => e.GoodCount);

            modelBuilder.Entity<Good>()
                .HasMany(e => e.SalePos)
                .WithRequired(e => e.Good)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Good>()
                .HasMany(g => g.PhotoGoods)
                .WithOptional(p => p.Good)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Manufacturer>()
                .Property(e => e.ManufacturerName)
                .IsUnicode(false);

            modelBuilder.Entity<Sale>()
                .Property(e => e.Summa)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Sale>()
                .Property(e => e.Tax)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Sale>()
                .Property(e => e.SummaTax)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Sale>()
                .HasMany(e => e.SalePos)
                .WithRequired(e => e.Sale)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SalePos>()
                .Property(e => e.Summa)
                .HasPrecision(19, 4);

            modelBuilder.Entity<SalePos>()
                .Property(e => e.Tax)
                .HasPrecision(19, 4);

            modelBuilder.Entity<SalePos>()
                .Property(e => e.SummaTax)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Tax>()
                .Property(e => e.TaxValue)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PhotoGood>()
                .HasKey(p => p.PhotoId);

        }
    }
}
