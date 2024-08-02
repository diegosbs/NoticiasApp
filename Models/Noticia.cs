using NoticiasApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Noticia
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(250)]
    public string Titulo { get; set; }

    [Required]
    public string Texto { get; set; }

    [Required]
    public int UsuarioId { get; set; }

    [ForeignKey("UsuarioId")]
    public Usuario Usuario { get; set; }

    // Propriedade de navegação
    public ICollection<NoticiaTag> NoticiaTags { get; set; }

}
