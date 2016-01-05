using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.EFModel.Model
{
    [Table("CommentGood")]
    public partial class CommentGood
    {
        [Key]
        public int CommentId { get; set; }

        [Required]
        public string Comment { get; set; }

        public int GoodId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateCreate { get; set; }

        public int ParentComment { get; set; }

        [StringLength(100)]
        public string UserName { get; set; }

        public virtual Good Good { get; set; }
    }
}
