using localLib.Data;
using localLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace localLib.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("Admin/[controller]")]
    public class EdituriController : Controller
    {
        private readonly BibliotecaContext _context;
        private readonly IValidator<Editura> _validator;

        public EdituriController(BibliotecaContext context, IValidator<Editura> validator)
        {
            _context = context;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DenumireSortParm"] = String.IsNullOrEmpty(sortOrder) ? "denumire_desc" : "";
            ViewData["NumarCartiSortParm"] = sortOrder == "NumarCarti" ? "numar_carti_desc" : "NumarCarti";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var edituri = from e in _context.Edituri.Include(e => e.Carti)
                         select e;

            ViewBag.TotalCount = await edituri.CountAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                edituri = edituri.Where(e => e.Denumire.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "denumire_desc":
                    edituri = edituri.OrderByDescending(e => e.Denumire);
                    break;
                case "NumarCarti":
                    edituri = edituri.OrderBy(e => e.Carti.Count());
                    break;
                case "numar_carti_desc":
                    edituri = edituri.OrderByDescending(e => e.Carti.Count());
                    break;
                default:
                    edituri = edituri.OrderBy(e => e.Denumire);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<Editura>.CreateAsync(edituri.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EdituraId,Denumire")] Editura editura)
        {
            var validationResult = await _validator.ValidateAsync(editura);
            
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(editura);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Editura a fost adăugată cu succes!";
                return RedirectToAction(nameof(Index));
            }

            return View(editura);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editura = await _context.Edituri
                .Include(e => e.Carti)
                .ThenInclude(c => c.Autori)
                .ThenInclude(ca => ca.Autor)
                .Include(e => e.Carti)
                .ThenInclude(c => c.ZonaColectie)
                .FirstOrDefaultAsync(m => m.EdituraId == id);

            if (editura == null)
            {
                return NotFound();
            }

            return View(editura);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editura = await _context.Edituri
                .Include(e => e.Carti)
                .FirstOrDefaultAsync(e => e.EdituraId == id);

            if (editura == null)
            {
                return NotFound();
            }

            return View(editura);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("EdituraId,Denumire")] Editura editura)
        {
            if (id != editura.EdituraId)
            {
                return NotFound();
            }

            var validationResult = await _validator.ValidateAsync(editura);
            
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(editura);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Editura a fost actualizată cu succes!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EdituraExists(editura.EdituraId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = editura.EdituraId });
            }

            editura = await _context.Edituri
                .Include(e => e.Carti)
                .FirstOrDefaultAsync(e => e.EdituraId == id);

            return View(editura);
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            var editura = await _context.Edituri
                .Include(e => e.Carti)
                .FirstOrDefaultAsync(e => e.EdituraId == id);

            if (editura == null)
            {
                return NotFound();
            }

            if (editura.Carti?.Any() == true)
            {
                TempData["ErrorMessage"] = "Nu se poate șterge editura deoarece are cărți asociate. Mutați mai întâi cărțile la altă editură.";
                return RedirectToAction(nameof(Details), new { id = id });
            }

            _context.Edituri.Remove(editura);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Editura a fost ștearsă cu succes!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return Json(new List<object>());
            }

            var edituri = await _context.Edituri
                .Where(e => e.Denumire.Contains(term))
                .Select(e => new { 
                    id = e.EdituraId, 
                    text = e.Denumire,
                    numarCarti = e.Carti.Count()
                })
                .Take(10)
                .ToListAsync();

            return Json(edituri);
        }

        private bool EdituraExists(long id)
        {
            return _context.Edituri.Any(e => e.EdituraId == id);
        }
    }
}