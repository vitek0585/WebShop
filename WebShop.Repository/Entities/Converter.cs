namespace WebShop.Repository
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Converter")]
    public partial class Converter
    {
        public int ConverterId { get; set; }

        [Column(TypeName = "date")]
        public DateTime CurrentDate { get; set; }

        [Column(TypeName = "money")]
        public decimal OneUsd { get; set; }

        [Column(TypeName = "money")]
        public decimal Hryvnia { get; set; }
    }
}
