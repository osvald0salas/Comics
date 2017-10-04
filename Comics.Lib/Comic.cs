using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Comics.Lib
{
    [Table("Comics")]
    public class Comic
    {
        public Comic(){}

        [Key]
        public int ID { get; set; }

        [Column("Name", TypeName = "nvarchar")]
        [MaxLength(50)]
        [Required]
        [Display(Name = "Comic Name")]
        public string Name { get; set; }

        [Column("Issue", TypeName = "nvarchar")]
        [MaxLength(20)]
        [Required]
        public string Issue { get; set; }

        [Column("Publisher", TypeName = "nvarchar")]
        [MaxLength(50)]
        [Required]
        public string Publisher { get; set; }

        [Column("Price", TypeName = "decimal")]
        public decimal Price { get; set; } = 0;

        [Column("Thumbnail", TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Thumbnail { get; set; }
    }
}
