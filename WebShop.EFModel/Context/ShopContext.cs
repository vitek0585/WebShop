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

       
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ClassificationGood> ClassificationGoods { get; set; }
        public virtual DbSet<Color> Colors { get; set; }

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
                .HasMany(g => g.ClassificationGoods)
                .WithRequired(c => c.Good)
                .HasForeignKey(c => c.GoodId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Good>()
                .HasMany(g => g.Image)
                .WithRequired(c => c.Good)
                .HasForeignKey(c => c.GoodId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ClassificationGood>()
                .HasMany(e => e.SalePoses)
                .WithOptional(s=>s.ClassificationGood)
                .HasForeignKey(s=>s.ClassificationId)
                .WillCascadeOnDelete(false);
   


            modelBuilder.Entity<Sale>()
                .HasMany(e => e.SalePos)
                .WithRequired(e => e.Sale)
                .HasForeignKey(e=>e.SaleId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SalePos>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

           modelBuilder.Entity<Category>()
           .HasOptional(x => x.Parent)
           .WithMany(x => x.Children)
           .HasForeignKey(x => x.ParentId)
           .WillCascadeOnDelete(false);

           modelBuilder.Entity<Category>()
         .HasMany(x => x.Goods)
         .WithOptional(g=>g.Category)
         .HasForeignKey(g=>g.CategoryId);

    
        }
    }
}
