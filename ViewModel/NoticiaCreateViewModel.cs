using System.ComponentModel.DataAnnotations;

namespace NoticiasApp.ViewModel
{
    public class NoticiaCreateViewModel
    {
        [Required]
        [MaxLength(250)]
        public string Titulo { get; set; }

        [Required]
        public string Texto { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        public int[] SelectedTags { get; set; } // Tags selecionadas
    }
}