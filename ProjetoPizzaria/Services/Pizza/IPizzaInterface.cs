using ProjetoPizzaria.Dto;
using ProjetoPizzaria.Models;

namespace ProjetoPizzaria.Services.Pizza
{
    public interface IPizzaInterface
    {
        Task<PizzaModel> CriarPizza(PizzaCriacaoDto pizzaDto, IFormFile foto);
        Task<List<PizzaModel>> GetPizzas();
        Task<PizzaModel>GetPizzaById(int id);
        Task<PizzaModel> EditarPizza(PizzaModel pizza, IFormFile? foto);
        Task<PizzaModel> DeletePizzaById(int id);
        Task<List<PizzaModel>> GetPizzasFilter(string pesquisar);
    }
}
