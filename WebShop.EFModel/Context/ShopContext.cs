using WebShop.EFModel.Model;

namespace WebShop.EFModel.Context
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ShopContext : DbContext
    {
        public ShopContext()
            : base("name=ShopContext")
        {
        }

        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ClassificationGood> ClassificationGoods { get; set; }
        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<CommentGood> CommentGoods { get; set; }
        public virtual DbSet<CategoryName> CategoryNames { get; set; }
        public virtual DbSet<CategoryType> CategoryTypes { get; set; }
        public virtual DbSet<Good> Goods { get; set; }
        public virtual DbSet<Image> Image { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<SalePos> SalePos { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }
        public virtual DbSet<ExchangeRates> Rates { get; set; } 

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Good>()
                .Property(e => e.PriceUsd)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Good>()
                .HasMany(g=>g.ClassificationGoods)
                .WithRequired(c=>c.Good)
                .WillCascadeOnDelete(true);
          
            

            modelBuilder.Entity<SalePos>()
                .HasOptional(e => e.ClassificationGood)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sale>()
                .Property(e => e.Summa)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Sale>()
                .HasMany(e => e.SalePos)
                .WithRequired(e => e.Sale)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SalePos>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

           modelBuilder.Entity<Category>()
           .HasOptional(x => x.Parent)
           .WithMany(x => x.Children)
           .HasForeignKey(x => x.ParentId)
           .WillCascadeOnDelete(false);
        }
    }
}
