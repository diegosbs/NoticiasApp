using System.ComponentModel.DataAnnotations;

namespace NoticiasApp.ViewModel
{
    public class TagViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Descricao { get; set; }
    }
}
