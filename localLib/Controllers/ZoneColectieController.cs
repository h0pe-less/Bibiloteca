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
    public class ZoneColectieController : Controller
    {
        private readonly BibliotecaContext _context;
        private readonly IValidator<ZonaColectie> _validator;

        public ZoneColectieController(BibliotecaContext context, IValidator<ZonaColectie> validator)
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

            var zoneColectie = from z in _context.ZoneColectie.Include(z => z.Carti)
                              select z;

            // Get total count for display
            ViewBag.TotalCount = await zoneColectie.CountAsync();

            // Search functionality
            if (!String.IsNullOrEmpty(searchString))
            {
                zoneColectie = zoneColectie.Where(z => z.DenumireZona.Contains(searchString));
            }

            // Sorting
            switch (sortOrder)
            {
                case "denumire_desc":
                    zoneColectie = zoneColectie.OrderByDescending(z => z.DenumireZona);
                    break;
                case "NumarCarti":
                    zoneColectie = zoneColectie.OrderBy(z => z.Carti.Count());
                    break;
                case "numar_carti_desc":
                    zoneColectie = zoneColectie.OrderByDescending(z => z.Carti.Count());
                    break;
                default:
                    zoneColectie = zoneColectie.OrderBy(z => z.DenumireZona);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<ZonaColectie>.CreateAsync(zoneColectie.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ZonaColectieId,DenumireZona")] ZonaColectie zonaColectie)
        {
            // Use FluentValidation
            var validationResult = await _validator.ValidateAsync(zonaColectie);
            
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(zonaColectie);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Zona de colecție a fost adăugată cu succes!";
                return RedirectToAction(nameof(Index));
            }

            return View(zonaColectie);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zonaColectie = await _context.ZoneColectie
                .Include(z => z.Carti)
                .ThenInclude(c => c.Editura)
                .Include(z => z.Carti)
                .ThenInclude(c => c.Autori)
                .ThenInclude(ca => ca.Autor)
                .FirstOrDefaultAsync(m => m.ZonaColectieId == id);

            if (zonaColectie == null)
            {
                return NotFound();
            }

            return View(zonaColectie);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zonaColectie = await _context.ZoneColectie
                .Include(z => z.Carti)
                .FirstOrDefaultAsync(z => z.ZonaColectieId == id);

            if (zonaColectie == null)
            {
                return NotFound();
            }

            return View(zonaColectie);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ZonaColectieId,DenumireZona")] ZonaColectie zonaColectie)
        {
            if (id != zonaColectie.ZonaColectieId)
            {
                return NotFound();
            }

            // Use FluentValidation
            var validationResult = await _validator.ValidateAsync(zonaColectie);
            
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
                    _context.Update(zonaColectie);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Zona de colecție a fost actualizată cu succes!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZonaColectieExists(zonaColectie.ZonaColectieId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = zonaColectie.ZonaColectieId });
            }

            // Reload related data if validation fails
            zonaColectie = await _context.ZoneColectie
                .Include(z => z.Carti)
                .FirstOrDefaultAsync(z => z.ZonaColectieId == id);

            return View(zonaColectie);
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            var zonaColectie = await _context.ZoneColectie
                .Include(z => z.Carti)
                .FirstOrDefaultAsync(z => z.ZonaColectieId == id);

            if (zonaColectie == null)
            {
                return NotFound();
            }

            // Check if zone has associated books
            if (zonaColectie.Carti?.Any() == true)
            {
                TempData["ErrorMessage"] = "Nu se poate șterge zona de colecție deoarece are cărți asociate. Mutați mai întâi cărțile în altă zonă.";
                return RedirectToAction(nameof(Details), new { id = id });
            }

            _context.ZoneColectie.Remove(zonaColectie);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Zona de colecție a fost ștearsă cu succes!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return Json(new List<object>());
            }

            var zone = await _context.ZoneColectie
                .Where(z => z.DenumireZona.Contains(term))
                .Select(z => new { 
                    id = z.ZonaColectieId, 
                    text = z.DenumireZona,
                    numarCarti = z.Carti.Count()
                })
                .Take(10)
                .ToListAsync();

            return Json(zone);
        }

        private bool ZonaColectieExists(long id)
        {
            return _context.ZoneColectie.Any(e => e.ZonaColectieId == id);
        }
    }
}