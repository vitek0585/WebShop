using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.EFModel.Model
{
    [Table("Good")]
    public partial class Good
    {
        public Good()
        {
            ClassificationGoods = new HashSet<ClassificationGood>();
            CommentGoods = new HashSet<CommentGood>();
            Image = new HashSet<Image>();
            SalePos = new HashSet<SalePos>();
        }

        public int GoodId { get; set; }

        [Required]
       public string GoodNameRu { get; set; }
        [Required]
        public string GoodNameEn { get; set; }
        public decimal PriceUsd { get; set; }
        public string Articul { get; set; }

        public string DescriptionRu { get; set; }

        public string DescriptionEn { get; set; }

        public int? Discount { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateCreate { get; set; }


        public int? CategoryId { get; set; }
   
        public virtual Category Category { get; set; }

        public virtual ICollection<ClassificationGood> ClassificationGoods { get; set; }

        public virtual ICollection<CommentGood> CommentGoods { get; set; }

        public virtual ICollection<Image> Image { get; set; }

        public virtual ICollection<SalePos> SalePos { get; set; }
    }
}
