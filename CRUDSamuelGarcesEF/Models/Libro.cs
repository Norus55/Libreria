using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRUDSamuelGarcesEF.Models;

public partial class Libro
{

    public string Isbn { get; set; } = null!;

    public string Titulo { get; set; } = null!;

    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    [Display(Name = "Publicación")] 
    public DateOnly? Publicacion { get; set; }

    public DateOnly? FechaRegistro { get; set; }

    public int? CodigoCategoria { get; set; }

    public int? NitEditorial { get; set; }

    [Display(Name = "Codigo Categoría")]
    public virtual Categoria? CodigoCategoriaNavigation { get; set; }

    public virtual ICollection<LibrosAutor> LibrosAutors { get; set; } = new List<LibrosAutor>();

    [Display(Name = "Nit Editorial")]
    public virtual Editoriale? NitEditorialNavigation { get; set; }
}
