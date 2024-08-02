using System.ComponentModel.DataAnnotations;

namespace NoticiasApp.ViewModel
{
    public class NoticiaEditViewModel
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(250)]
        public string Titulo { get; set; }
        [Required]
        public string Texto { get; set; }
        [Required]
        public int UsuarioId { get; set; }
        public List<int> SelectedTags { get; set; }
    }

}