using localLib.Data;
using localLib.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace localLib.Controllers
{
    public class CartiController : Controller
    {
        private readonly BibliotecaContext _context;

        public CartiController(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var carti = await _context.Carti
                    .Include(c => c.Editura)
                    .Include(c => c.Limba)
                    .Include(c => c.Tara)
                    .Include(c => c.ZonaColectie)
                    .Include(c => c.Autori)
                        .ThenInclude(ca => ca.Autor)
                    .Include(c => c.CarteCategorii)
                        .ThenInclude(cc => cc.Categorie)
                    .OrderBy(c => c.Titlu)
                    .ToListAsync();

                return View(carti);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
                return View(new List<Carte>());
            }
        }

        public IActionResult Create()
        {
            ViewData["EdituraId"] = new SelectList(_context.Edituri, "EdituraId", "Denumire");
            ViewData["LimbaId"] = new SelectList(_context.Limbi, "LimbaId", "DenumireLimba");
            ViewData["TaraId"] = new SelectList(_context.Tari, "TaraId", "DenumireTara");
            ViewData["ZonaColectieId"] = new SelectList(_context.ZoneColectie, "ZonaColectieId", "DenumireZona");
            ViewData["NumePrenume"] = new SelectList(_context.Autori, "NumePrenume");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
            ViewData["NumePrenume"] = new SelectList(_context.Autori, "NumePrenume");
            return View(carte);
        }

    }
}
