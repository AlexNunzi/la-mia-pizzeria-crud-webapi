using la_mia_pizzeria_static.Database;
using la_mia_pizzeria_static.Migrations;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            List<Pizza> allPizza = _pizzaDatabase.Pizza.Include(p => p.Ingredients).Include(p => p.Category).ToList();
            if(allPizza != null && allPizza.Count > 0)
            {
                return Ok(allPizza);
            } else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult SearchPizzas(string? searchingName)
        {
            if(searchingName != null && searchingName.Trim().Length > 0) 
            {
                List<Pizza> allPizza = _pizzaDatabase.Pizza.Where(p => p.Name.ToLower().Contains(searchingName.Trim().ToLower())).Include(p => p.Ingredients).Include(p => p.Category).ToList();
                if (allPizza != null && allPizza.Count > 0)
                {
                    return Ok(allPizza);
                }
                else
                {
                    return GetPizzas();
                }
            } else
            {
                return GetPizzas();
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetPizzaById(long id)
        {
            Pizza? foundPizza = _pizzaDatabase.Pizza.Include(p => p.Ingredients).Include(p => p.Category).Where(p => p.Id == id).FirstOrDefault();
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

        [HttpPut("{id}")]
        public IActionResult UpdatePizza(long id, [FromBody] PizzaFormModel newPizzaModel)
        {
            try
            {
                Pizza? updatingPizza = _pizzaDatabase.Pizza.Where(p => p.Id == id).Include(pizza => pizza.Ingredients).FirstOrDefault();
                if(updatingPizza != null)
                {
                    if (newPizzaModel.Pizza.UrlImage == null)
                    {
                        newPizzaModel.Pizza.UrlImage = "/img/pizza_placeholder.png";
                    }
                    updatingPizza.Name = newPizzaModel.Pizza.Name;
                    updatingPizza.Description = newPizzaModel.Pizza.Description;
                    updatingPizza.UrlImage = newPizzaModel.Pizza.UrlImage;
                    updatingPizza.Price = newPizzaModel.Pizza.Price;
                    updatingPizza.CategoryId = newPizzaModel.Pizza.CategoryId;
                    updatingPizza.Ingredients?.Clear();
                    if (newPizzaModel.SelectedIngredients != null)
                    {
                        updatingPizza.Ingredients = new List<Ingredient>();
                        foreach (String ingredientId in newPizzaModel.SelectedIngredients)
                        {
                            long parsedIngredientId = long.Parse(ingredientId);

                            Ingredient? dbIngredient = _pizzaDatabase.Ingredients.Where(i => i.Id == parsedIngredientId).FirstOrDefault();

                            if (dbIngredient != null)
                            {
                                updatingPizza.Ingredients.Add(dbIngredient);
                            }
                        }
                    }
                    _pizzaDatabase.SaveChanges();
                    return Ok();
                } else
                {
                    return BadRequest();
                }
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePizzaById(long id)
        {
            try
            {
                Pizza deletingPizza = _pizzaDatabase.Pizza.Find(id);
                if(deletingPizza != null)
                {
                    _pizzaDatabase.Remove(deletingPizza);
                    _pizzaDatabase.SaveChanges();
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetCategoriesList()
        {
            List<Category> allCategories = _pizzaDatabase.Categories.ToList();
            if (allCategories != null && allCategories.Count > 0)
            {
                return Ok(allCategories);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult GetIngredientsList()
        {
            List<Ingredient> allIngredients = _pizzaDatabase.Ingredients.ToList();
            if (allIngredients != null && allIngredients.Count > 0)
            {
                return Ok(allIngredients);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
