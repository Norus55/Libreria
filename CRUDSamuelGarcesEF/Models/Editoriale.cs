using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRUDSamuelGarcesEF.Models;

public partial class Editoriale
{
    public int Nit { get; set; }

    public string? Nombres { get; set; }

    public string? Telefono { get; set; }

    [Display(Name = "Descripción")]
    public string? Direccion { get; set; }

    public string? Email { get; set; }

    public string? Sitioweb { get; set; }

    public virtual ICollection<Libro> Libros { get; set; } = new List<Libro>();
}
