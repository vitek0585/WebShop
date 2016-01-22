using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.EFModel.Model
{
    [Table("Brand")]
    public partial class Brand
    {
        

        public int BrandId { get; set; }

        [Required]
        [StringLength(100)]
        public string BrandName { get; set; }

    }
}
