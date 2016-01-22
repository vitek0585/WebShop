﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebShop.App_GlobalResources;

namespace WebShop.Models
{
    public class UserOrder
    {
        [Range(1, 100, ErrorMessageResourceName = "DataError", ErrorMessageResourceType = typeof(Resource))]
        public int SizeId { get; set; }
        
        [Range(1, 100, ErrorMessageResourceName = "DataError", ErrorMessageResourceType = typeof(Resource))]
        public int ColorId { get; set; }
        
        [Range(1, 1000000, ErrorMessageResourceName = "DataError", ErrorMessageResourceType = typeof(Resource))]
        public int GoodId { get; set; }

        [Range(1, 10000, ErrorMessageResourceName = "DataError", ErrorMessageResourceType = typeof(Resource))]
        public int CountGood { get; set; }
        public decimal PriceWithDiscount { get; set; }
        public int Discount { get; set; }
        public int ClassificationId { get; set; }
        public string ColorNameEn { get; set; }
        public string SizeName { get; set; }
        public IEnumerable<string> Photos { get; set; }
        public string GoodName { get; set; }
        public decimal PriceUsd { get; set; }

    }
}