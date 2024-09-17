using Microsoft.AspNetCore.Mvc;
using ProjetoPizzaria.Services.Pizza;
using System.Diagnostics;

namespace ProjetoPizzaria.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPizzaInterface _pizzaInterface;
        public HomeController(IPizzaInterface pizzaInterface)
        {
            _pizzaInterface = pizzaInterface;
        }
        public async Task<IActionResult> Index(string? pesquisar)
        {
            if (string.IsNullOrEmpty(pesquisar))
            {
                var pizzas = await _pizzaInterface.GetPizzas();
                return View(pizzas);
            }
            else
            {
                var pizzas = await _pizzaInterface.GetPizzasFilter(pesquisar);
                return View(pizzas);
            }
        }
    }
}
