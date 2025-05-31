using localLib.Data;
using localLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace localLib.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BibliotecaContext _context;

        public HomeController(BibliotecaContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize (Roles = "Admin")] 
        [Route("Admin")]
        public IActionResult Admin()
        {
            return View();
        }

        public async Task<IActionResult> Search(
            string searchQuery = "",
            string sortBy = "Relevanta",
            List<string> limbi = null,
            List<string> categorii = null,
            List<string> tari = null,
            int? anMinim = null,
            int? anMaxim = null,
            int page = 1)
        {

            limbi ??= new List<string>();
            categorii ??= new List<string>();
            tari ??= new List<string>();

            ViewBag.SearchQuery = searchQuery;
            ViewBag.SortBy = sortBy;
            ViewBag.SelectedLimbi = limbi ?? new List<string>();
            ViewBag.SelectedCategorii = categorii ?? new List<string>();
            ViewBag.SelectedTari = tari ?? new List<string>();
            ViewBag.AnMinim = anMinim;
            ViewBag.AnMaxim = anMaxim;

            ViewBag.Languages = await _context.Limbi.Select(l => l.DenumireLimba).Distinct().ToListAsync();
            ViewBag.Categories = await _context.Categorii.Select(c => c.Denumire).Distinct().ToListAsync();
            ViewBag.Countries = await _context.Tari.Select(e => e.DenumireTara).Distinct().ToListAsync();

            var query = _context.Carti
                .Include(c => c.Editura)
                .Include(c => c.Limba)
                .Include(c => c.CarteCategorii)
                .Include(c => c.Autori)
                    .ThenInclude(ca => ca.Autor)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(c =>
                    c.Titlu.Contains(searchQuery) ||
                    c.TitluInfo.Contains(searchQuery) ||
                    c.Autori.Any(a => a.Autor.NumePrenume.Contains(searchQuery)));
            }

            if (limbi != null && limbi.Any())
            {
                query = query.Where(c => limbi.Contains(c.Limba.DenumireLimba));
            }

            if (tari != null && tari.Any())
            {
                query = query.Where(c => tari.Contains(c.Tara.DenumireTara));
            }

            if (anMinim.HasValue)
            {
                query = query.Where(c => c.DataPublicarii.Year >= anMinim.Value);
            }
            if (anMaxim.HasValue)
            {
                query = query.Where(c => c.DataPublicarii.Year <= anMaxim.Value);
            }

            query = sortBy switch
            {
                "Cele mai noi" => query.OrderByDescending(c => c.DataPublicarii),
                "Cele mai vechi" => query.OrderBy(c => c.DataPublicarii),
                "Titlu A-Z" => query.OrderBy(c => c.Titlu),
                "Titlu Z-A" => query.OrderByDescending(c => c.Titlu),
                _ => query.OrderByDescending(c => c.NumarInventar)
            };

            int totalCount = await query.CountAsync();

            int pageSize = 10;
            var paginatedList = await PaginatedList<Carte>.CreateAsync(query.AsNoTracking(), page, pageSize);

            ViewBag.TotalCount = totalCount;
            ViewBag.CurrentPage = paginatedList.PageIndex;
            ViewBag.TotalPages = paginatedList.TotalPages;
            ViewBag.Limbi = await _context.Limbi.ToListAsync();

            return View(paginatedList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}