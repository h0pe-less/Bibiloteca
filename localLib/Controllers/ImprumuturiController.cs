using localLib.Data;
using localLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace localLib.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Route("Admin/[controller]")]
    public class ImprumuturiController : Controller
    {
        private readonly BibliotecaContext _context;

        public ImprumuturiController(BibliotecaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber, string statusFilter)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NumeSortParm"] = String.IsNullOrEmpty(sortOrder) ? "nume_desc" : "";
            ViewData["PrenumeSortParm"] = sortOrder == "Prenume" ? "prenume_desc" : "Prenume";
            ViewData["GrupaSortParm"] = sortOrder == "Grupa" ? "grupa_desc" : "Grupa";
            ViewData["DataImprumutSortParm"] = sortOrder == "DataImprumut" ? "data_imprumut_desc" : "DataImprumut";
            ViewData["DataReturnareSort"] = sortOrder == "DataReturnare" ? "data_returnare_desc" : "DataReturnare";
            ViewData["StatusSortParm"] = sortOrder == "Status" ? "status_desc" : "Status";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            ViewData["StatusFilter"] = statusFilter;

            var imprumuturi = from i in _context.Imprumuturi.Include(i => i.Carte)
                              select i;

            // Search functionality
            if (!String.IsNullOrEmpty(searchString))
            {
                imprumuturi = imprumuturi.Where(i => i.Nume.Contains(searchString)
                                               || i.Prenume.Contains(searchString)
                                               || i.Grupa.Contains(searchString)
                                               || i.Carte.Titlu.Contains(searchString));
            }

            // Status filter
            if (!String.IsNullOrEmpty(statusFilter))
            {
                switch (statusFilter.ToLower())
                {
                    case "returnat":
                        imprumuturi = imprumuturi.Where(i => i.EsteReturnat == true);
                        break;
                    case "neretornat":
                        imprumuturi = imprumuturi.Where(i => i.EsteReturnat == false);
                        break;
                    case "intarziat":
                        imprumuturi = imprumuturi.Where(i => i.EsteReturnat == false && i.DataReturnare < DateTime.Now);
                        break;
                }
            }

            // Sorting
            switch (sortOrder)
            {
                case "nume_desc":
                    imprumuturi = imprumuturi.OrderByDescending(i => i.Nume);
                    break;
                case "Prenume":
                    imprumuturi = imprumuturi.OrderBy(i => i.Prenume);
                    break;
                case "prenume_desc":
                    imprumuturi = imprumuturi.OrderByDescending(i => i.Prenume);
                    break;
                case "Grupa":
                    imprumuturi = imprumuturi.OrderBy(i => i.Grupa);
                    break;
                case "grupa_desc":
                    imprumuturi = imprumuturi.OrderByDescending(i => i.Grupa);
                    break;
                case "DataImprumut":
                    imprumuturi = imprumuturi.OrderBy(i => i.DataImprumut);
                    break;
                case "data_imprumut_desc":
                    imprumuturi = imprumuturi.OrderByDescending(i => i.DataImprumut);
                    break;
                case "DataReturnare":
                    imprumuturi = imprumuturi.OrderBy(i => i.DataReturnare);
                    break;
                case "data_returnare_desc":
                    imprumuturi = imprumuturi.OrderByDescending(i => i.DataReturnare);
                    break;
                case "Status":
                    imprumuturi = imprumuturi.OrderBy(i => i.EsteReturnat);
                    break;
                case "status_desc":
                    imprumuturi = imprumuturi.OrderByDescending(i => i.EsteReturnat);
                    break;
                default:
                    imprumuturi = imprumuturi.OrderBy(i => i.Nume);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<Imprumut>.CreateAsync(imprumuturi.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            ViewData["CarteId"] = new SelectList(_context.Carti, "CarteId", "Titlu");
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImprumutId,Nume,Prenume,Grupa,DataImprumut,CarteId")] Imprumut imprumut)
        {
            ModelState.Remove("Carte");

            if (ModelState.IsValid)
            {
                _context.Add(imprumut);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CarteId"] = new SelectList(_context.Carti, "CarteId", "Titlu", imprumut.CarteId);
            return View(imprumut);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imprumut = await _context.Imprumuturi
                .Include(i => i.Carte)
                .FirstOrDefaultAsync(m => m.ImprumutId == id);

            if (imprumut == null)
            {
                return NotFound();
            }

            return View(imprumut);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imprumut = await _context.Imprumuturi.FindAsync(id);
            if (imprumut == null)
            {
                return NotFound();
            }
            ViewData["CarteId"] = new SelectList(_context.Carti, "CarteId", "Titlu", imprumut.CarteId);
            return View(imprumut);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ImprumutId,Nume,Prenume,Grupa,DataImprumut,DataReturnare,CarteId,EsteReturnat,DataReturnareEfectiva")] Imprumut imprumut)
        {
            if (id != imprumut.ImprumutId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(imprumut);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImprumutExists(imprumut.ImprumutId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarteId"] = new SelectList(_context.Carti, "CarteId", "Titlu", imprumut.CarteId);
            return View(imprumut);
        }

        [HttpPost("Return/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(long id)
        {
            var imprumut = await _context.Imprumuturi.FindAsync(id);
            if (imprumut == null)
            {
                return NotFound();
            }

            imprumut.EsteReturnat = true;

            _context.Update(imprumut);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imprumut = await _context.Imprumuturi
                .Include(i => i.Carte)
                .FirstOrDefaultAsync(m => m.ImprumutId == id);

            if (imprumut == null)
            {
                return NotFound();
            }

            return View(imprumut);
        }

        [HttpPost("Delete/{id}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var imprumut = await _context.Imprumuturi.FindAsync(id);
            if (imprumut != null)
            {
                _context.Imprumuturi.Remove(imprumut);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ImprumutExists(long id)
        {
            return _context.Imprumuturi.Any(e => e.ImprumutId == id);
        }
    }
}