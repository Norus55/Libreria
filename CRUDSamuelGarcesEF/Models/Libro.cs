using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRUDSamuelGarcesEF.Models;

public partial class Libro
{

    [Required(ErrorMessage = "El isbn es requerido")]
    [Display(Name = "ISBN")]
    public string Isbn { get; set; } = null!;

    [Required(ErrorMessage = "El Título del libro es requerido")]
    [Display(Name = "Título")]
    public string Titulo { get; set; } = null!;

    [Required(ErrorMessage = "La descripción del libro es requerida")]
    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "La fecha de publicación del libro es requerida")]
    [Display(Name = "Fecha de publicación")] 
    public DateOnly? Publicacion { get; set; }

    [Required(ErrorMessage = "La fecha de registro del libro es requerida")]
    [Display(Name = "Fecha de registro")]
    public DateOnly? FechaRegistro { get; set; }

    public int? CodigoCategoria { get; set; }

    public int? NitEditorial { get; set; }

    [Display(Name = "Codigo Categoría")]
    public virtual Categoria? CodigoCategoriaNavigation { get; set; }

    public virtual ICollection<LibrosAutor> LibrosAutors { get; set; } = new List<LibrosAutor>();

    [Display(Name = "Nit Editorial")]
    public virtual Editorial? NitEditorialNavigation { get; set; }
}
