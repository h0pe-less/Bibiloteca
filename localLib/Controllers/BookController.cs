using Microsoft.AspNetCore.Mvc;
using localLib.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using localLib.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing.Printing;
using FluentValidation;

namespace localLib.Controllers;

public class BookController : Controller
{
    private readonly ILogger<BookController> _logger;

    private readonly BibliotecaContext _context;

    public BookController(BibliotecaContext context, ILogger<BookController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [Route("Admin/Book")]
    public async Task<IActionResult> Index(
        string sortOrder,
        string currentFilter,
        string searchString,
        int? zonaFilter,
        int? pageNumber)
    {
        ViewData["CurrentSort"] = sortOrder;
        ViewData["TitluSortParm"] = string.IsNullOrEmpty(sortOrder) ? "titlu_desc" : "";
        ViewData["AnSortParm"] = sortOrder == "an" ? "an_desc" : "an";
        ViewData["AutorSortParm"] = sortOrder == "autor" ? "autor_desc" : "autor";

        if (searchString != null)
        {
            pageNumber = 1;
        }
        else
        {
            searchString = currentFilter;
        }

        ViewData["CurrentFilter"] = searchString;
        ViewData["ZonaFilter"] = zonaFilter;

        var carti = _context.Carti
            .Include(c => c.Editura)
            .Include(c => c.ZonaColectie)
            .Include(c => c.Limba)
            .Include(c => c.Tara)
            .Include(c => c.Autori).ThenInclude(ca => ca.Autor)
            .Include(c => c.CarteCategorii).ThenInclude(cc => cc.Categorie)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            carti = carti.Where(c =>
                c.Titlu.Contains(searchString) ||
                c.ISBN.Contains(searchString) ||
                c.Autori.Any(a => a.Autor.NumePrenume.Contains(searchString)) ||
                (c.Editura != null && c.Editura.Denumire.Contains(searchString)));
        }
        if (zonaFilter.HasValue)
        {
            carti = carti.Where(c => c.ZonaColectieId == zonaFilter);
        }

        switch (sortOrder)
        {
            case "titlu_desc":
                carti = carti.OrderByDescending(c => c.Titlu);
                break;
            case "an":
                carti = carti.OrderBy(c => c.DataPublicarii);
                break;
            case "an_desc":
                carti = carti.OrderByDescending(c => c.DataPublicarii);
                break;
            case "autor":
                carti = carti.OrderBy(c => c.Autori.FirstOrDefault()!.Autor.NumePrenume);
                break;
            case "autor_desc":
                carti = carti.OrderByDescending(c => c.Autori.FirstOrDefault()!.Autor.NumePrenume);
                break;
            default:
                carti = carti.OrderBy(c => c.Titlu);
                break;
        }

        ViewBag.ZoneColectie = await _context.ZoneColectie.ToListAsync();

        int pageSize = 10;
        return View(await PaginatedList<Carte>.CreateAsync(carti.AsNoTracking(), pageNumber ?? 1, pageSize));
    }

    public IActionResult Search()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    //[Authorize (Roles = "Admin")] 
    //[Route("Admin/Book")]
    //public IActionResult Admin()
    //{
    //    return View("Index");
    //}

    [HttpGet]
    [Route("Admin/Book/Create")]
    //[Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        ViewData["EdituraId"] = new SelectList(_context.Edituri, "EdituraId", "Denumire");
        ViewData["LimbaId"] = new SelectList(_context.Limbi, "LimbaId", "DenumireLimba");
        ViewData["TaraId"] = new SelectList(_context.Tari, "TaraId", "DenumireTara");
        ViewData["ZonaColectieId"] = new SelectList(_context.ZoneColectie, "ZonaColectieId", "DenumireZona");
        ViewData["NumePrenume"] = new SelectList(_context.Autori, "AutorId", "NumePrenume");
        ViewData["Denumire"] = new SelectList(_context.Categorii, "CategorieId", "Denumire");

        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("Admin/Book/Create")]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([Bind("CarteId,ISBN,ISSN,Cota,Titlu,TitluInfo,MentiuniResponsabilitate,Editie,EdituraId,DataPublicarii,LoculPublicarii,Bibliografie,Descriere,NrPagini,Pret,ZonaColectieId,LimbaId,TaraId,NumarInventar,Paginatie,Ilustratii,CopertaURL")] Carte carte,
                                     string selectedAuthorIds,
                                     string selectedCategoryIds)
    {
        if (ModelState.IsValid)
        {
            _context.Add(carte);
            await _context.SaveChangesAsync();

            if (!string.IsNullOrEmpty(selectedAuthorIds))
            {
                var authorIds = selectedAuthorIds.Split(',').Select(long.Parse).ToList();
                foreach (var authorId in authorIds)
                {
                    _context.CartiAutori.Add(new CarteAutor { CarteId = carte.CarteId, AutorId = authorId });
                }
            }

            if (!string.IsNullOrEmpty(selectedCategoryIds))
            {
                var categoryIds = selectedCategoryIds.Split(',').Select(long.Parse).ToList();
                foreach (var categoryId in categoryIds)
                {
                    _context.CartiCategorii.Add(new CarteCategorie { CarteId = carte.CarteId, CategorieId = categoryId });
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["EdituraId"] = new SelectList(_context.Edituri, "EdituraId", "Denumire", carte.EdituraId);
        ViewData["LimbaId"] = new SelectList(_context.Limbi, "LimbaId", "DenumireLimba", carte.LimbaId);
        ViewData["TaraId"] = new SelectList(_context.Tari, "TaraId", "DenumireTara", carte.TaraId);
        ViewData["ZonaColectieId"] = new SelectList(_context.ZoneColectie, "ZonaColectieId", "DenumireZona", carte.ZonaColectieId);
        ViewData["NumePrenume"] = new SelectList(_context.Autori, "AutorId", "NumePrenume");
        ViewData["Denumire"] = new SelectList(_context.Categorii, "CategorieId", "Denumire");

        return View(carte);
    }

    [Route("Admin/Book/Details/{id}")]
    public async Task<IActionResult> Details(long? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var carte = await _context.Carti
            .Include(c => c.Editura)
            .Include(c => c.Limba)
            .Include(c => c.Tara)
            .Include(c => c.ZonaColectie)
            .Include(c => c.Autori)
                .ThenInclude(ca => ca.Autor)
            .Include(c => c.CarteCategorii)
                .ThenInclude(cc => cc.Categorie)
            .FirstOrDefaultAsync(m => m.CarteId == id);

        if (carte == null)
        {
            return NotFound();
        }

        return View(carte);
    }

    [HttpGet]
    [Route("Admin/Book/Edit/{id}")]
    public async Task<IActionResult> Edit(long? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var carte = await _context.Carti
            .Include(c => c.Autori)
            .Include(c => c.CarteCategorii)
            .FirstOrDefaultAsync(c => c.CarteId == id);

        if (carte == null)
        {
            return NotFound();
        }

        var selectedAuthorIds = carte.Autori?.Select(a => a.AutorId.ToString()).ToList();
        ViewBag.SelectedAuthorIds = string.Join(",", selectedAuthorIds ?? new List<string>());

        var selectedCategoryIds = carte.CarteCategorii?.Select(cc => cc.CategorieId.ToString()).ToList();
        ViewBag.SelectedCategoryIds = string.Join(",", selectedCategoryIds ?? new List<string>());

        ViewData["EdituraId"] = new SelectList(_context.Edituri, "EdituraId", "Denumire", carte.EdituraId);
        ViewData["LimbaId"] = new SelectList(_context.Limbi, "LimbaId", "DenumireLimba", carte.LimbaId);
        ViewData["TaraId"] = new SelectList(_context.Tari, "TaraId", "DenumireTara", carte.TaraId);
        ViewData["ZonaColectieId"] = new SelectList(_context.ZoneColectie, "ZonaColectieId", "DenumireZona", carte.ZonaColectieId);
        ViewData["NumePrenume"] = new SelectList(_context.Autori, "AutorId", "NumePrenume");
        ViewData["Denumire"] = new SelectList(_context.Categorii, "CategorieId", "Denumire");

        return View(carte);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("Admin/Book/Edit/{id}")]
    public async Task<IActionResult> Edit(long id,
    [Bind("CarteId,ISBN,ISSN,Cota,Titlu,TitluInfo,MentiuniResponsabilitate,Editie,EdituraId,DataPublicarii,LoculPublicarii,Bibliografie,Descriere,NrPagini,Pret,ZonaColectieId,LimbaId,TaraId,NumarInventar,Paginatie,Ilustratii,CopertaURL")] Carte carte,
    string selectedAuthorIds,
    string selectedCategoryIds)
    {
        if (id != carte.CarteId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(carte);

                var existingAuthors = await _context.CartiAutori
                    .Where(ca => ca.CarteId == id)
                    .ToListAsync();
                _context.CartiAutori.RemoveRange(existingAuthors);

                if (!string.IsNullOrEmpty(selectedAuthorIds))
                {
                    var authorIds = selectedAuthorIds.Split(',').Select(long.Parse).ToList();
                    foreach (var authorId in authorIds)
                    {
                        _context.CartiAutori.Add(new CarteAutor { CarteId = carte.CarteId, AutorId = authorId });
                    }
                }

                var existingCategories = await _context.CartiCategorii
                    .Where(cc => cc.CarteId == id)
                    .ToListAsync();
                _context.CartiCategorii.RemoveRange(existingCategories);

                if (!string.IsNullOrEmpty(selectedCategoryIds))
                {
                    var categoryIds = selectedCategoryIds.Split(',').Select(long.Parse).ToList();
                    foreach (var categoryId in categoryIds)
                    {
                        _context.CartiCategorii.Add(new CarteCategorie { CarteId = carte.CarteId, CategorieId = categoryId });
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarteExists(carte.CarteId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        ViewData["EdituraId"] = new SelectList(_context.Edituri, "EdituraId", "Denumire", carte.EdituraId);
        ViewData["LimbaId"] = new SelectList(_context.Limbi, "LimbaId", "DenumireLimba", carte.LimbaId);
        ViewData["TaraId"] = new SelectList(_context.Tari, "TaraId", "DenumireTara", carte.TaraId);
        ViewData["ZonaColectieId"] = new SelectList(_context.ZoneColectie, "ZonaColectieId", "DenumireZona", carte.ZonaColectieId);
        ViewData["NumePrenume"] = new SelectList(_context.Autori, "AutorId", "NumePrenume");
        ViewData["Denumire"] = new SelectList(_context.Categorii, "CategorieId", "Denumire");

        return View(carte);
    }

    private bool CarteExists(long id)
    {
        return _context.Carti.Any(e => e.CarteId == id);
    }

    [HttpGet]
    [Route("Admin/Book/Delete/{id}")]
    public async Task<IActionResult> Delete(long? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var carte = await _context.Carti
            .Include(c => c.Editura)
            .Include(c => c.ZonaColectie)
            .Include(c => c.Limba)
            .Include(c => c.Tara)
            .FirstOrDefaultAsync(m => m.CarteId == id);

        if (carte == null)
        {
            return NotFound();
        }

        return View(carte);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Route("Admin/Book/Delete/{id}")]
    public async Task<IActionResult> DeleteConfirmed(long id)
    {
        var carte = await _context.Carti
            .Include(c => c.Autori)
            .Include(c => c.CarteCategorii)
            .FirstOrDefaultAsync(c => c.CarteId == id);

        if (carte == null)
        {
            return NotFound();
        }

        try
        {
            _context.CartiAutori.RemoveRange(carte.Autori);
            _context.CartiCategorii.RemoveRange(carte.CarteCategorii);

            _context.Carti.Remove(carte);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error deleting book");
            ModelState.AddModelError("", "Nu se poate șterge cartea. Există înregistrări dependente.");
            return View("Delete", carte);
        }
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}