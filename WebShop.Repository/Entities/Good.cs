using System.Linq;

namespace WebShop.Repository
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Good")]
    public partial class Good
    {

        public Good()
        {
            SalePos = new HashSet<SalePos>();
            PhotoGoods = new HashSet<PhotoGood>();
        }

        public int GoodId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Bad name should be min 3, max 100 characters")]
        public string GoodName { get; set; }

        public int? ManufacturerId { get; set; }
        [Range(0, 100000, ErrorMessage = "The Price should be in the range of from 0 to 100000")]
        [Column(TypeName = "money")]
        public decimal PriceUsd { get; set; }

        [Column(TypeName = "numeric")]
        [Range(0, 100000, ErrorMessage = "The count should be in the range of from 0 to 100000")]
        public int GoodCount { get; set; }

        public virtual Manufacturer Manufacturer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalePos> SalePos { get; set; }

        public virtual ICollection<PhotoGood> PhotoGoods { get; set; }
    }

    public static class HelperGood
    {
        public static int? TryGetFirstPhotoId(this Good good)
        {
            int? id = null;
            if (good.PhotoGoods.Any())
                id = good.PhotoGoods.First().PhotoId;

            return id;
        }
    }
}
