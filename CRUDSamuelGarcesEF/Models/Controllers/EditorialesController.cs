using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUDSamuelGarcesEF.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace CRUDSamuelGarcesEF.Controllers
{
    public class EditorialesController : Controller
    {
        private readonly Mvccrud2Context _context;
        private readonly JsonSerializerOptions _jsonOptions;

        public EditorialesController(Mvccrud2Context context)
        {
            _context = context;
            _jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
        }

        // GET: Editoriales
        public async Task<IActionResult> Index()
        {
            return View(await _context.Editoriales.ToListAsync());
        }

        // GET: Editoriales/Details/5
        [HttpGet]
        public async Task<JsonResult> Details(int id)
        {
            var editorial = await _context.Editoriales
                .FirstOrDefaultAsync(m => m.Nit == id);

            if (editorial == null)
            {
                return Json(new { success = false, message = "Editorial no encontrada" });
            }

            var editorialDto = new
            {
                nit = editorial.Nit,
                nombres = editorial.Nombres,
                telefono = editorial.Telefono,
                direccion = editorial.Direccion,
                email = editorial.Email,
                sitioWeb = editorial.Sitioweb
            };

            return Json(new { success = true, data = editorialDto });
        }

        // GET: Editoriales/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Editoriales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Create([FromForm] Editorial editorial)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (ModelState.IsValid)
                    {
                        //verificar si ya existe el código
                        if (EditorialeExists(editorial.Nit))
                        {
                            return Json(new { success = false, message = "El NIT ya existe" });
                        }
                        //verificar si el nombre ya existe
                        if (_context.Editoriales.Any(c => c.Nombres == editorial.Nombres))
                        {
                            return Json(new { success = false, message = "Ya existe una categoría con ese nombre" });

                        }
                        if (_context.Editoriales.Any(c => c.Email == editorial.Email))
                        {
                            return Json(new { success = false, message = "Ese email ya está registrado" });
                        }
                        if (_context.Editoriales.Any(c => c.Direccion == editorial.Direccion))
                        {
                            return Json(new { success = false, message = "Esa dirección ya está registrada" });
                        }
                        if (_context.Editoriales.Any(c => c.Direccion == editorial.Direccion))
                        {
                            return Json(new { success = false, message = "Ese teléfono ya está registrado" });
                        }

                        _context.Add(editorial);
                        await _context.SaveChangesAsync();
                        return Json(new { success = true, message = "Editoria creada exitosamente" });
                    }
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al crear la editorial: " + ex.Message });
            }

            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return Json(new { success = false, message = "Datos inválidos", errors });
        }

        // GET: Editoriales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editoriale = await _context.Editoriales.FindAsync(id);
            if (editoriale == null)
            {
                return NotFound();
            }
            return View(editoriale);
        }

        // POST: Editoriales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(int id, [FromForm] Editorial editorial)
        {
            if (id != editorial.Nit)
            {
                return Json(new { success = false, message = "ID no coincide" });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {

                        //verificar si el nombre ya existe
                        if (_context.Editoriales.Any(c => c.Nombres == editorial.Nombres))
                        {
                            return Json(new { success = false, message = "Ya existe una categoría con ese nombre" });

                        }
                        if (_context.Editoriales.Any(c => c.Email == editorial.Email))
                        {
                            return Json(new { success = false, message = "Ese email ya está registrado" });
                        }
                        if (_context.Editoriales.Any(c => c.Direccion == editorial.Direccion))
                        {
                            return Json(new { success = false, message = "Esa dirección ya está registrada" });
                        }
                        if (_context.Editoriales.Any(c => c.Direccion == editorial.Direccion))
                        {
                            return Json(new { success = false, message = "Ese teléfono ya está registrado" });
                        }

                        _context.Add(editorial);
                        await _context.SaveChangesAsync();
                        return Json(new { success = true, message = "Categoría creada exitosamente" });
                    }
                    _context.Update(editorial);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Editorial actualizada exitosamente" });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EditorialeExists(editorial.Nit))
                    {
                        return Json(new { success = false, message = "Editorial no encontrada" });
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

        // GET: Editoriales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editoriale = await _context.Editoriales
                .FirstOrDefaultAsync(m => m.Nit == id);
            if (editoriale == null)
            {
                return NotFound();
            }

            return View(editoriale);
        }

        // POST: Editoriales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteAjax(int id)
        {
            var editorial = await _context.Editoriales.FindAsync(id);
            if (editorial == null)
            {
                return Json(new { success = false, message = "La editorial no se encontró." });
            }

            // Verificar si hay libros asociados a esta editorial
            if (_context.Libros.Any(l => l.NitEditorial == editorial.Nit)) 
            {
                return Json(new { success = false, message = "No se puede eliminar la editorial porque tiene libros asignados." });
            }

            _context.Editoriales.Remove(editorial);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "La editorial se eliminó con éxito." });
        }
        private bool EditorialeExists(int id)
        {
            return _context.Editoriales.Any(e => e.Nit == id);
        }
    }
}
