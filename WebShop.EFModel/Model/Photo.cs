using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.EFModel.Model
{
    [Table("Photo")]
    public partial class Photo
    {
        public int PhotoId { get; set; }

        public int GoodId { get; set; }

        [Required]
        public byte[] PhotoByte { get; set; }

        [Required]
        [StringLength(100)]
        public string PhotoName { get; set; }

        [Required]
        [StringLength(100)]
        public string MimeType { get; set; }

        public virtual Good Good { get; set; }
    }
}
