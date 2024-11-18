using System;
using System.Collections.Generic;

namespace CRUDSamuelGarcesEF.Models;

public partial class LibrosAutor
{
    public int IdLibrosAutor { get; set; }

    public int? IdAutor { get; set; }

    public string? Isbn { get; set; }

    public virtual Autor? IdAutorNavigation { get; set; }

    public virtual Libro? IsbnNavigation { get; set; }
}
