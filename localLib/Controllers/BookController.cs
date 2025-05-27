using Microsoft.AspNetCore.Mvc;
using localLib.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using localLib.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public async Task<IActionResult> Index()
    {
        var books = await _context.Carti
            .Include(c => c.Editura)
            .Include(c => c.ZonaColectie)
            .Include(c => c.Limba)
            .Include(c => c.Tara)
            .Include(c => c.Autori).ThenInclude(ca => ca.Autor)
            .Include(c => c.CarteCategorii).ThenInclude(cc => cc.Categorie)
            .ToListAsync();

        return View(books);
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
    public async Task<IActionResult> Create([Bind("CarteId,ISBN,ISSN,Cota,Titlu,TitluInfo,MentiuniResponsabilitate,Editie,EdituraId,DataPublicarii,LoculPublicarii,Bibliografie,Descriere,NrPagini,Pret,ZonaColectieId,LimbaId,TaraId,NumarInventar,CopertaURL")] Carte carte)
    {
        if (ModelState.IsValid)
        {
            _context.Add(carte);
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
    public IActionResult Details()
    {
        return View();
    }

    [Route("Admin/Book/Edit/{id}")]
    public IActionResult Edit()
    {
        return View();
    }

    [Route("Admin/Book/Delete/{id}")]
    public IActionResult Delete()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}