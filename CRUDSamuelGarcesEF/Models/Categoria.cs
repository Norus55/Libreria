using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRUDSamuelGarcesEF.Models;

public partial class Categoria
{
    [Required(ErrorMessage="El código de categoría es requerido")]
    [Display(Name = "Código")]
    public int CodigoCategoria { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(60,ErrorMessage="No puede ser de más de 60 carácteres")]
    public string? Nombre { get; set; }

    public virtual ICollection<Libro> Libros { get; set; } = new List<Libro>();
}
