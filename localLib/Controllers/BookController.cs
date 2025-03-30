using Microsoft.AspNetCore.Mvc;
using localLib.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
namespace localLib.Controllers;

    public class BookController : Controller
    {
        private readonly ILogger<BookController> _logger;

        public BookController(ILogger<BookController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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
        [Route("Admin/Book")]
        public IActionResult Admin()
        {
            return View("Index");
        }

        [Route("Admin/Book/Create")]
        public IActionResult Create()
        {
            return View();
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