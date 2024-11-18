using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRUDSamuelGarcesEF.Models;

public class NumericAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is int)
        {
            return ValidationResult.Success;
        }

        if (value is string strValue)
        {
            if (int.TryParse(strValue, out _))
            {
                return ValidationResult.Success;
            }
        }

        return new ValidationResult(ErrorMessage ?? "El valor debe ser un número entero");
    }
}

public partial class Editorial
{
    [Required(ErrorMessage = "El NIT es requerido")]
    [Range(1, int.MaxValue, ErrorMessage = "El NIT debe ser un número positivo.")]
    [Numeric(ErrorMessage = "El NIT debe ser un número entero.")]
    public int Nit { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    [Display(Name = "Nombre")]
    public string  Nombres { get; set; }

    [Required(ErrorMessage = "El teléfono es requerido")]
    [Display(Name = "Teléfono")]
    public string  Telefono { get; set; }

    [Required(ErrorMessage = "La dirección es requerida")]
    [Display(Name = "Dirección")]
    public string  Direccion { get; set; }

    [Required(ErrorMessage = "El email es requerido")]
    [Display(Name = "Email")]
    public string  Email { get; set; }

    [Required(ErrorMessage = "El Sitio web es requerido")]
    [Display(Name = "Sitio web")]
    public string  Sitioweb { get; set; }

    public virtual ICollection<Libro> Libros { get; set; } = new List<Libro>();
}
