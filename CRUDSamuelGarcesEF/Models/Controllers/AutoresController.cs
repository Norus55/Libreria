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
    public class AutoresController : Controller
    {
        private readonly Mvccrud2Context _context;
        private readonly JsonSerializerOptions _jsonOptions;

        public AutoresController(Mvccrud2Context context)
        {
            _context = context;
            _jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
        }

        // GET: Autores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Autors.ToListAsync());
        }

        // GET: Autores/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var autor = await _context.Autors
        //        .FirstOrDefaultAsync(m => m.IdAutor == id);
        //    if (autor == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(autor);
        //}

        [HttpGet]
        public async Task<JsonResult> Details(int id)
        {
            var autor = await _context.Autors
                .FirstOrDefaultAsync(m => m.IdAutor == id);

            if (autor == null)
            {
                return Json(new { success = false, message = "Autor no encontrado" });
            }

            var autorDto = new
            {
                idAutor = autor.IdAutor,
                nombre = autor.Nombre,
                apellido = autor.Apellido,
                nacionalidad = autor.Nacionalidad
            };

            return Json(new { success = true, data = autorDto });
        }

        // GET: Autores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Autores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("IdAutor,Nombre,Apellido,Nacionalidad")] Autor autor)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(autor);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(autor);
        //}
        [HttpPost]
        public async Task<JsonResult> Create([FromForm] Autor autor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(autor);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Autor creado exitosamente" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error al crear el autor: " + ex.Message });
                }
            }

            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return Json(new { success = false, message = "Datos inválidos", errors });
        }

        // GET: Autores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autors.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }
            return View(autor);
        }

        // POST: Autores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("IdAutor,Nombre,Apellido,Nacionalidad")] Autor autor)
        //{
        //    if (id != autor.IdAutor)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(autor);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!AutorExists(autor.IdAutor))
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
        //    return View(autor);
        //}

        [HttpPost]
        public async Task<JsonResult> Edit(int id, [FromForm] Autor autor)
        {
            if (id != autor.IdAutor)
            {
                return Json(new { success = false, message = "ID no coincide" });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(autor);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Autor actualizado exitosamente" });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutorExists(autor.IdAutor))
                    {
                        return Json(new { success = false, message = "Autor no encontrado" });
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


        // GET: Autores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autors
                .FirstOrDefaultAsync(m => m.IdAutor == id);
            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        // POST: Autores/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var autor = await _context.Autors.FindAsync(id);
        //    if (autor != null)
        //    {
        //        _context.Autors.Remove(autor);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
        [HttpPost]
        public async Task<JsonResult> DeleteAjax(int id)
        {
            var autor = await _context.Autors.FindAsync(id);
            if (autor == null)
            {
                return Json(new { success = false, message = "El autor no se encontró." });
            }

            _context.Autors.Remove(autor);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "El autor se eliminó con éxito." });
        }

        private bool AutorExists(int id)
        {
            return _context.Autors.Any(e => e.IdAutor == id);
        }
    }

}
