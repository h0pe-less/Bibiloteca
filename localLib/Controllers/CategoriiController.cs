using localLib.Data;
using localLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace localLib.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Route("Admin/[controller]")]
    public class CategoriiController : Controller
    {
        private readonly BibliotecaContext _context;
        private readonly IValidator<Categorie> _validator;

        public CategoriiController(BibliotecaContext context, IValidator<Categorie> validator)
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

            var categorii = from c in _context.Categorii.Include(c => c.CartiCategorii)
                           select c;

            // Get total count for display
            ViewBag.TotalCount = await categorii.CountAsync();

            // Search functionality
            if (!String.IsNullOrEmpty(searchString))
            {
                categorii = categorii.Where(c => c.Denumire.Contains(searchString) 
                                              || c.Descriere.Contains(searchString));
            }

            // Sorting
            switch (sortOrder)
            {
                case "denumire_desc":
                    categorii = categorii.OrderByDescending(c => c.Denumire);
                    break;
                case "NumarCarti":
                    categorii = categorii.OrderBy(c => c.CartiCategorii.Count());
                    break;
                case "numar_carti_desc":
                    categorii = categorii.OrderByDescending(c => c.CartiCategorii.Count());
                    break;
                default:
                    categorii = categorii.OrderBy(c => c.Denumire);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<Categorie>.CreateAsync(categorii.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategorieId,Denumire,Descriere")] Categorie categorie)
        {
            // Use FluentValidation
            var validationResult = await _validator.ValidateAsync(categorie);
            
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(categorie);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Categoria a fost adăugată cu succes!";
                return RedirectToAction(nameof(Index));
            }

            return View(categorie);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categorie = await _context.Categorii
                .Include(c => c.CartiCategorii)
                .ThenInclude(cc => cc.Carte)
                .ThenInclude(c => c.Editura)
                .Include(c => c.CartiCategorii)
                .ThenInclude(cc => cc.Carte)
                .ThenInclude(c => c.Autori)
                .ThenInclude(ca => ca.Autor)
                .FirstOrDefaultAsync(m => m.CategorieId == id);

            if (categorie == null)
            {
                return NotFound();
            }

            return View(categorie);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categorie = await _context.Categorii
                .Include(c => c.CartiCategorii)
                .ThenInclude(cc => cc.Carte)
                .FirstOrDefaultAsync(c => c.CategorieId == id);

            if (categorie == null)
            {
                return NotFound();
            }

            return View(categorie);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CategorieId,Denumire,Descriere")] Categorie categorie)
        {
            if (id != categorie.CategorieId)
            {
                return NotFound();
            }

            // Use FluentValidation
            var validationResult = await _validator.ValidateAsync(categorie);
            
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
                    _context.Update(categorie);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Categoria a fost actualizată cu succes!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategorieExists(categorie.CategorieId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = categorie.CategorieId });
            }

            // Reload related data if validation fails
            categorie = await _context.Categorii
                .Include(c => c.CartiCategorii)
                .ThenInclude(cc => cc.Carte)
                .FirstOrDefaultAsync(c => c.CategorieId == id);

            return View(categorie);
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            var categorie = await _context.Categorii
                .Include(c => c.CartiCategorii)
                .FirstOrDefaultAsync(c => c.CategorieId == id);

            if (categorie == null)
            {
                return NotFound();
            }

            // Check if category has associated books
            if (categorie.CartiCategorii?.Any() == true)
            {
                TempData["ErrorMessage"] = "Nu se poate șterge categoria deoarece are cărți asociate. Eliminați mai întâi cărțile din această categorie.";
                return RedirectToAction(nameof(Details), new { id = id });
            }

            _context.Categorii.Remove(categorie);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Categoria a fost ștearsă cu succes!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return Json(new List<object>());
            }

            var categorii = await _context.Categorii
                .Where(c => c.Denumire.Contains(term) || c.Descriere.Contains(term))
                .Select(c => new { 
                    id = c.CategorieId, 
                    text = c.Denumire,
                    descriere = c.Descriere,
                    numarCarti = c.CartiCategorii.Count()
                })
                .Take(10)
                .ToListAsync();

            return Json(categorii);
        }

        private bool CategorieExists(long id)
        {
            return _context.Categorii.Any(e => e.CategorieId == id);
        }
    }
}