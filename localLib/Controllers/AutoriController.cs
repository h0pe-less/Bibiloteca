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
    public class AutoriController : Controller
    {
        private readonly BibliotecaContext _context;
        private readonly IValidator<Autor> _validator;

        public AutoriController(BibliotecaContext context, IValidator<Autor> validator)
        {
            _context = context;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NumeSortParm"] = String.IsNullOrEmpty(sortOrder) ? "nume_desc" : "";
            ViewData["DataNasteriiSortParm"] = sortOrder == "DataNasterii" ? "data_nasterii_desc" : "DataNasterii";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var autori = from a in _context.Autori.Include(a => a.CartiAutor)
                         select a;

            // Get total count for display
            ViewBag.TotalCount = await autori.CountAsync();

            // Search functionality
            if (!String.IsNullOrEmpty(searchString))
            {
                autori = autori.Where(a => a.NumePrenume.Contains(searchString)
                                     || a.Biografie.Contains(searchString));
            }

            // Sorting
            switch (sortOrder)
            {
                case "nume_desc":
                    autori = autori.OrderByDescending(a => a.NumePrenume);
                    break;
                case "DataNasterii":
                    autori = autori.OrderBy(a => a.DataNasterii);
                    break;
                case "data_nasterii_desc":
                    autori = autori.OrderByDescending(a => a.DataNasterii);
                    break;
                default:
                    autori = autori.OrderBy(a => a.NumePrenume);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<Autor>.CreateAsync(autori.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AutorId,NumePrenume,Biografie,DataNasterii")] Autor autor)
        {
            // Use FluentValidation
            var validationResult = await _validator.ValidateAsync(autor);
            
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(autor);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Autorul a fost adăugat cu succes!";
                return RedirectToAction(nameof(Index));
            }

            return View(autor);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autori
                .Include(a => a.CartiAutor)
                .ThenInclude(ca => ca.Carte)
                .ThenInclude(c => c.Editura)
                .FirstOrDefaultAsync(m => m.AutorId == id);

            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autori
                .Include(a => a.CartiAutor)
                .ThenInclude(ca => ca.Carte)
                .FirstOrDefaultAsync(a => a.AutorId == id);

            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("AutorId,NumePrenume,Biografie,DataNasterii")] Autor autor)
        {
            if (id != autor.AutorId)
            {
                return NotFound();
            }

            // Use FluentValidation
            var validationResult = await _validator.ValidateAsync(autor);
            
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
                    _context.Update(autor);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Autorul a fost actualizat cu succes!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutorExists(autor.AutorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = autor.AutorId });
            }

            // Reload related data if validation fails
            autor = await _context.Autori
                .Include(a => a.CartiAutor)
                .ThenInclude(ca => ca.Carte)
                .FirstOrDefaultAsync(a => a.AutorId == id);

            return View(autor);
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            var autor = await _context.Autori
                .Include(a => a.CartiAutor)
                .FirstOrDefaultAsync(a => a.AutorId == id);

            if (autor == null)
            {
                return NotFound();
            }

            // Check if author has associated books
            if (autor.CartiAutor?.Any() == true)
            {
                TempData["ErrorMessage"] = "Nu se poate șterge autorul deoarece are cărți asociate. Ștergeți mai întâi cărțile.";
                return RedirectToAction(nameof(Details), new { id = id });
            }

            _context.Autori.Remove(autor);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Autorul a fost șters cu succes!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return Json(new List<object>());
            }

            var autori = await _context.Autori
                .Where(a => a.NumePrenume.Contains(term))
                .Select(a => new { 
                    id = a.AutorId, 
                    text = a.NumePrenume,
                    dataNasterii = a.DataNasterii.ToString("yyyy-MM-dd")
                })
                .Take(10)
                .ToListAsync();

            return Json(autori);
        }

        private bool AutorExists(long id)
        {
            return _context.Autori.Any(e => e.AutorId == id);
        }
    }
}