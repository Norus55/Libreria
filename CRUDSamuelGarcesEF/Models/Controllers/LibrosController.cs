using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUDSamuelGarcesEF.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CRUDSamuelGarcesEF.Controllers
{
    public class LibrosController : Controller
    {
        private readonly Mvccrud2Context _context;
        private readonly JsonSerializerOptions _jsonOptions;

        public LibrosController(Mvccrud2Context context)
        {
            _context = context;
            _context = context;
            _jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };

        }

        // GET: Libroes
        public async Task<IActionResult> Index()
        {
            var mvccrud2Context = _context.Libros.Include(l => l.CodigoCategoriaNavigation).Include(l => l.NitEditorialNavigation);
            return View(await mvccrud2Context.ToListAsync());
        }

        // GET: Libroes/Details/5
        [HttpGet]
        public async Task<JsonResult> Details(string id)
        {
            var libro = await _context.Libros
                .Include(l => l.CodigoCategoriaNavigation)
                .Include(l => l.NitEditorialNavigation)
                .FirstOrDefaultAsync(m => m.Isbn == id);

            if (libro == null)
            {
                return Json(new { success = false, message = "Libro no encontrado" });
            }

            // Crear un objeto anónimo con solo los datos necesarios
            var libroDto = new
            {
                isbn = libro.Isbn,
                titulo = libro.Titulo,
                descripcion = libro.Descripcion,
                publicacion = libro.Publicacion?.ToString("dd/MM/yyyy"),
                fechaRegistro = libro.FechaRegistro?.ToString("dd/MM/yyyy"),
                categoria = libro.CodigoCategoriaNavigation?.Nombre ?? "Sin categoría",
                editorial = libro.NitEditorialNavigation?.Nombres ?? "Sin editorial",
                codigoCategoria = libro.CodigoCategoria,
                nitEditorial = libro.NitEditorial
            };

            return Json(new { success = true, data = libroDto });
        }
        //public async Task<IActionResult> Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var libro = await _context.Libros
        //        .Include(l => l.CodigoCategoriaNavigation)
        //        .Include(l => l.NitEditorialNavigation)
        //        .FirstOrDefaultAsync(m => m.Isbn == id);
        //    if (libro == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(libro);
        //}

        // GET: Libroes/Create
        public IActionResult Create()
        {
            ViewData["CodigoCategoria"] = new SelectList(_context.Categorias, "CodigoCategoria", "CodigoCategoria");
            ViewData["NitEditorial"] = new SelectList(_context.Editoriales, "Nit", "Nit");
            return View();
        }

        // POST: Libroes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598
        [HttpPost]
        public async Task<JsonResult> Create([FromForm] Libro libro)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(libro);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Libro creado exitosamente" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error al crear el libro: " + ex.Message });
                }
            }

            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return Json(new { success = false, message = "Datos inválidos", errors });
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Isbn,Titulo,Descripcion,Publicacion,FechaRegistro,CodigoCategoria,NitEditorial")] Libro libro)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(libro);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CodigoCategoria"] = new SelectList(_context.Categorias, "CodigoCategoria", "CodigoCategoria", libro.CodigoCategoria);
        //    ViewData["NitEditorial"] = new SelectList(_context.Editoriales, "Nit", "Nit", libro.NitEditorial);
        //    return View(libro);
        //}

        // GET: Libroes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }
            ViewData["CodigoCategoria"] = new SelectList(_context.Categorias, "CodigoCategoria", "CodigoCategoria", libro.CodigoCategoria);
            ViewData["NitEditorial"] = new SelectList(_context.Editoriales, "Nit", "Nit", libro.NitEditorial);
            return View(libro);
        }

        // POST: Libroes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<JsonResult> Edit(string id, [FromForm] Libro libro)
        {
            if (id != libro.Isbn)
            {
                return Json(new { success = false, message = "ID no coincide" });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(libro);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Libro actualizado exitosamente" });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibroExists(libro.Isbn))
                    {
                        return Json(new { success = false, message = "Libro no encontrado" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Error de concurrencia" });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error al actualizar: " + ex.Message });
                }
            }

            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return Json(new { success = false, message = "Datos inválidos", errors });
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, [Bind("Isbn,Titulo,Descripcion,Publicacion,FechaRegistro,CodigoCategoria,NitEditorial")] Libro libro)
        //{
        //    if (id != libro.Isbn)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(libro);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!LibroExists(libro.Isbn))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CodigoCategoria"] = new SelectList(_context.Categorias, "CodigoCategoria", "CodigoCategoria", libro.CodigoCategoria);
        //    ViewData["NitEditorial"] = new SelectList(_context.Editoriales, "Nit", "Nit", libro.NitEditorial);
        //    return View(libro);
        //}

        // GET: Libroes/Delete/5
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var libro = await _context.Libros
        //        .Include(l => l.CodigoCategoriaNavigation)
        //        .Include(l => l.NitEditorialNavigation)
        //        .FirstOrDefaultAsync(m => m.Isbn == id);
        //    if (libro == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(libro);
        //}

        // POST: Libroes/Delete/5
        [HttpPost]
        public async Task<JsonResult> DeleteAjax(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { success = false, message = "El ID del libro no puede ser nulo." });
            }

            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return Json(new { success = false, message = "El libro no se encontró." });
            }

            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "El libro se eliminó con éxito." });
        }


        // POST: Libroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro != null)
            {
                _context.Libros.Remove(libro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibroExists(string id)
        {
            return _context.Libros.Any(e => e.Isbn == id);
        }
    }
}
