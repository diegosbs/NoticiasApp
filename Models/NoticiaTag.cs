using NoticiasApp.Models;
using System.ComponentModel.DataAnnotations;

public class NoticiaTag
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required]
    public int TagId { get; set; }
    [Required]
    public int NoticiaId { get; set; }
    [Required]
    public Tag Tag { get; set; }
    [Required]
    public Noticia Noticia { get; set; }
}
