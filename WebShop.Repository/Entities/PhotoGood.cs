using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.Repository
{
    [Table("PhotoGood")]
    public class PhotoGood
    {
        public int PhotoId { get; set; }
        public string PhotoName { get; set; }
        public string MimeType{ get; set; }
        public byte[] Photo { get; set; }
        public int? GoodId { get; set; }
        public Good Good { get; set; }


    }
}