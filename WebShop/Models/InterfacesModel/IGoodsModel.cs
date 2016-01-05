using System.Collections.Generic;

namespace WebShop.Models.InterfacesModel
{
    public interface IGoodsModel
    {
        int GoodId { get; set; }
        int CategoryId { get; set; }
        string CategoryName { get; set; }
        string TypeName { get; set; }
        string GoodName { get; set; }
        int GoodCount { get; set; }
        int Discount { get; set; }
        IEnumerable<string> Photos { get; set; }
        decimal PriceUsd { get; set; }
        
    }
}