using System.ComponentModel.DataAnnotations;

namespace NoticiasApp.Models;
public class Usuario
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(250)]
    public string Nome { get; set; }

    [Required]
    [MaxLength(50)]
    public string Senha { get; set; }

    [Required]
    [MaxLength(250)]
    public string Email { get; set; }

    // Propriedade de navegação
    public ICollection<Noticia> Noticias { get; set; }
}

