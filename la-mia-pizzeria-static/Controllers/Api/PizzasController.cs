using la_mia_pizzeria_static.Database;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace la_mia_pizzeria_static.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PizzasController : ControllerBase
    {
        private DbPizzeriaContext _pizzaDatabase;

        public PizzasController(DbPizzeriaContext pizzaDatabase)
        {
            _pizzaDatabase = pizzaDatabase;
        }

        [HttpGet]
        public IActionResult GetPizzas()
        {
            List<Pizza> allPizza = _pizzaDatabase.Pizza.ToList();
            if(allPizza != null && allPizza.Count > 0)
            {
                return Ok(allPizza);
            } else
            {
                return NotFound();
            }
        }

        [HttpGet("{searchingName}")]
        public IActionResult GetPizzas(string searchingName)
        {
            if(searchingName != null && searchingName.Trim().Length > 0) 
            {
                List<Pizza> allPizza = _pizzaDatabase.Pizza.Where(p => p.Name.ToLower().Contains(searchingName.Trim().ToLower())).ToList();
                if (allPizza != null && allPizza.Count > 0)
                {
                    return Ok(allPizza);
                }
                else
                {
                    return NotFound();
                }
            } else
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetPizzaById(long id)
        {
            Pizza? foundPizza = _pizzaDatabase.Pizza.Find(id);
            if (foundPizza != null)
            {
                return Ok(foundPizza);
            } else 
            { 
                return NotFound(); 
            }
        }

        [HttpPost]
        public IActionResult CreatePizza([FromBody] PizzaFormModel newPizzaModel)
        {
            try
            {
                if (newPizzaModel.Pizza.UrlImage == null)
                {
                    newPizzaModel.Pizza.UrlImage = "/img/pizza_placeholder.png";
                }

                if(newPizzaModel.SelectedIngredients != null)
                {
                    newPizzaModel.Pizza.Ingredients = new List<Ingredient>();
                    foreach (String ingredientId in newPizzaModel.SelectedIngredients)
                    {
                        long parsedIngredientId = long.Parse(ingredientId);

                        Ingredient? dbIngredient = _pizzaDatabase.Ingredients.Where(i => i.Id == parsedIngredientId).FirstOrDefault();

                        if (dbIngredient != null)
                        {
                            newPizzaModel.Pizza.Ingredients.Add(dbIngredient);
                        }
                    }
                }
                _pizzaDatabase.Add(newPizzaModel.Pizza);
                _pizzaDatabase.SaveChanges();
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
