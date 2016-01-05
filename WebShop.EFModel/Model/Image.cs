using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.EFModel.Model
{
     [Table("Image")]
    public class Image
    {
        public int ImageId { get; set; }
        public string ImagePath { get; set; }
        public int GoodId { get; set; }
        public virtual Good Good { get; set; }
    }
}