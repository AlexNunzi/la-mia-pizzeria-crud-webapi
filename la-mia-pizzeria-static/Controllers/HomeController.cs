using la_mia_pizzeria_static.Database;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace la_mia_pizzeria_static.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN,USER")]
        public IActionResult Index()
        {
            List<Pizza> pizzaList = new();

            using (DbPizzeriaContext db = new DbPizzeriaContext())
            {
                try
                {
                    pizzaList = db.Pizza.ToList();
                }
                catch (Exception ex)
                {
                    //  Gestione dell'errore
                }
            }

            return View("Index", pizzaList);
        }
        [HttpGet]
        public IActionResult UserIndex()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}