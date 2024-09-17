using Microsoft.EntityFrameworkCore;
using ProjetoPizzaria.Data;
using ProjetoPizzaria.Dto;
using ProjetoPizzaria.Models;

namespace ProjetoPizzaria.Services.Pizza
{
    public class PizzaService : IPizzaInterface
    {
        private readonly AppDbContext _context;
        private readonly string _sistema;
        public PizzaService(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _sistema = environment.WebRootPath;
        }

        public string GenerateAddressPhoto(IFormFile foto)
        {
            var uniqueCode = Guid.NewGuid().ToString();

            var addressFile = foto.FileName.Replace(" ", "").ToLower() + uniqueCode + ".png";

            var addressToSaveImage = _sistema + "\\image\\";

            if (!Directory.Exists(addressToSaveImage))
            {
                Directory.CreateDirectory(addressToSaveImage);
            }

            using var stream = File.Create(addressToSaveImage + addressFile);
            foto.CopyToAsync(stream).Wait();

            return addressFile;
        }

        public async Task<PizzaModel> CriarPizza(PizzaCriacaoDto pizzaDto, IFormFile foto)
        {
            try
            {
                var nomeCaminhoImagem = GenerateAddressPhoto(foto);

                var pizza = new PizzaModel()
                {
                    Sabor = pizzaDto.Sabor,
                    Descricao = pizzaDto.Descricao,
                    Valor = pizzaDto.Valor,
                    Capa = nomeCaminhoImagem
                };


                _context.Add(pizza);
                await _context.SaveChangesAsync();

                return pizza;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<PizzaModel>> GetPizzas()
        {
            try
            {
                return await _context.Pizzas.ToListAsync();
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PizzaModel> GetPizzaById(int id)
        {
            try
            {
                return await _context.Pizzas.FirstOrDefaultAsync(pizza => pizza.Id.Equals(id));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PizzaModel> EditarPizza(PizzaModel pizza, IFormFile? foto)
        {
            try
            {
                var pizzaReturn = await _context.Pizzas
                     .AsNoTracking()
                     .FirstOrDefaultAsync(pizzab => pizzab.Id == pizza.Id);

                var newPathPhoto = string.Empty;

                if (foto is not null) 
                {
                    string addressPhotoExisting = _sistema + "\\image\\" + pizzaReturn?.Capa;

                    if (File.Exists(addressPhotoExisting)) 
                    {
                        File.Delete(addressPhotoExisting);
                    }

                    newPathPhoto = GenerateAddressPhoto(foto);
                }

                pizzaReturn.Sabor = pizza.Sabor;
                pizzaReturn.Valor = pizza.Valor;
                pizzaReturn.Descricao = pizza.Descricao;

                if (!string.IsNullOrEmpty(newPathPhoto))
                {
                    pizzaReturn.Capa = newPathPhoto;
                }
                
                _context.Update(pizzaReturn);
                await _context.SaveChangesAsync();

                return pizzaReturn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PizzaModel> DeletePizzaById(int id)
        {
            try
            {
                var pizza = await _context.Pizzas.FirstOrDefaultAsync(x => x.Id == id);

                File.Delete(_sistema + "\\image\\" + pizza.Capa);

                _context.Remove(pizza);
                await _context.SaveChangesAsync();

                return pizza;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<PizzaModel>> GetPizzasFilter(string pesquisar)
        {
            try
            {
                var pizzas =  await _context.Pizzas
                    .Where(pizzaSearch => pizzaSearch.Sabor.Contains(pesquisar))
                    .ToListAsync();

                return pizzas;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
