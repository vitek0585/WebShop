namespace WebShop.Repository
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Number")]
    public partial class Number
    {
        public int NumberId { get; set; }

        public int NumberSale { get; set; }
    }
}
