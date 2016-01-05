using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.Models
{
   
    public class GoodHome
    {
        public int GoodId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Bad name should be min 3, max 100 characters")]
        public string GoodName { get; set; }

        [Range(0, 100000, ErrorMessage = "The count should be in the range of from 0 to 1000")]
        [Column(TypeName = "money")]
        public decimal PriceUsd { get; set; }

        [Column(TypeName = "numeric")]
        [Range(0, 100000, ErrorMessage = "The count should be in the range of from 0 to 1000")]
        public int GoodCount { get; set; }

        public string  PhotoPath { get; set; } 
    }
}