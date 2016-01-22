using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.EFModel.Model
{
    [Table("Sale")]
    public partial class Sale
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sale()
        {
            SalePos = new HashSet<SalePos>();
        }

        public int SaleId { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateSale { get; set; }


        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        public virtual ICollection<SalePos> SalePos { get; set; }
    }
}
