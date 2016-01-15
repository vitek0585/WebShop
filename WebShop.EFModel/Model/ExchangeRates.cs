using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.EFModel.Model
{
    [Table("ExchangeRates")]
    public class ExchangeRates
    {
        [Key]
        public int IdRate { get; set; }
        public decimal UsdRate { get; set; }
        public DateTime DateRate { get; set; }
    }
}