using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoticiasApp.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Descricao { get; set; }
        [NotMapped]
        public ICollection<NoticiaTag> NoticiaTags { get; set; }
    }

}
