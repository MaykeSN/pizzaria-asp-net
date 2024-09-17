using Microsoft.AspNetCore.Mvc;
using ProjetoPizzaria.Dto;
using ProjetoPizzaria.Models;
using ProjetoPizzaria.Services.Pizza;

namespace ProjetoPizzaria.Controllers
{
    public class PizzaController : Controller
    {
        private IPizzaInterface _pizzaInterface;
        public PizzaController(IPizzaInterface pizzaService)
        {
            _pizzaInterface = pizzaService;
        }
        public async Task<IActionResult> Index()
        {
            var pizzas = await _pizzaInterface.GetPizzas();
            return View(pizzas);
        }

        public IActionResult Cadastrar()
        {
            return View();
        }

        public async Task<IActionResult> Detalhes(int id)
        {
            var pizza = await _pizzaInterface.GetPizzaById(id);
            return View(pizza);
        }

        public async Task<IActionResult> Editar(int id)
        {
            var pizza = await _pizzaInterface.GetPizzaById(id);

            return View(pizza);
        }

        public async Task<IActionResult> Remover(int id)
        {
            var pizza = await _pizzaInterface.DeletePizzaById(id);
            return RedirectToAction("Index", "Pizza");
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(PizzaCriacaoDto pizzadto, IFormFile foto)
        {
            if (ModelState.IsValid)
            {
                var pizza = await _pizzaInterface.CriarPizza(pizzadto, foto);

                return RedirectToAction("Index", "Pizza");
            }
            else
            {
                return View(pizzadto);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Editar(PizzaModel pizzaModel, IFormFile? foto)
        {
            if (ModelState.IsValid)
            {
                var pizza = await _pizzaInterface.EditarPizza(pizzaModel, foto);
                return RedirectToAction("Index", "Pizza");
            }
            else
            {
                return View(pizzaModel);
            }
        }
    }
}
