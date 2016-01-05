using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.EFModel.Model
{
    [Table("Color")]
    public partial class Color
    {
        public Color()
        {
            ClassificationGoods = new HashSet<ClassificationGood>();
        }

        public int ColorId { get; set; }

        [Required]
        [StringLength(100)]
        public string ColorName { get; set; }

        public virtual ICollection<ClassificationGood> ClassificationGoods { get; set; }
    }
}
