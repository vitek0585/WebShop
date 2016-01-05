using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Models
{
    public class UserOrder
    {
        public int SizeId { get; set; }

        public int ColorId { get; set; }

        public int GoodId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Count good Error")]
        public int CountGood { get; set; }


        public int ClassificationId { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public IEnumerable<string> Photos { get; set; }
        public string GoodName { get; set; }
        public decimal PriceUsd { get; set; }

    }
}