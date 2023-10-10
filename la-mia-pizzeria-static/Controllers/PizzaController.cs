using la_mia_pizzeria_static.CustomLoggers;
using la_mia_pizzeria_static.Database;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace la_mia_pizzeria_static.Controllers
{
    public class PizzaController : Controller
    {

        private ICustomLogger _logger;
        private DbPizzeriaContext _pizzaDatabase;

        public PizzaController(ICustomLogger logger, DbPizzeriaContext pizzaDatabase)
        {
            this._logger = logger;
            this._pizzaDatabase = pizzaDatabase;
        }

        public IActionResult Menu()
        {

            List<Pizza> pizzaList = new();

            
            try
            {
                pizzaList = _pizzaDatabase.Pizza.ToList();
            }
            catch (Exception ex)
            {
                //  Gestione dell'errore
            }
            

            return View("Menu", pizzaList);
        }

        [Authorize(Roles = "ADMIN,USER")]
        public IActionResult Details(long id)
        {

            
            Pizza? foundPizza = _pizzaDatabase.Pizza.Where(pizza => pizza.Id == id).Include(pizza => pizza.Category).Include(pizza => pizza.Ingredients).FirstOrDefault();

            if (foundPizza == null)
            {
                _logger.WriteFileLog($"Tentativo fallito di mostrare dettagli della pizza con id = {id}");
                return NotFound($"Non è stata trovata una pizza con id={id}.");
            }
            else
            {
                return View("Details", foundPizza);
            }
            

        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public IActionResult Create()
        {

            List<Category> categories = _pizzaDatabase.Categories.ToList();

            PizzaFormModel model = new PizzaFormModel();

            List<Ingredient> dbIngredients = _pizzaDatabase.Ingredients.ToList();

            List<SelectListItem> availableIngredients = new List<SelectListItem>();

            if(dbIngredients != null)
            {
                foreach(Ingredient ingredient in dbIngredients)
                {
                    string ingredientStringId = ingredient.Id.ToString();

                    availableIngredients.Add(new SelectListItem { Value = ingredientStringId, Text = ingredient.Name });
                }
            }

            model.Ingredients = availableIngredients;

            model.Pizza = new Pizza();
            model.Categories = categories;

            return View("Create", model);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaFormModel pizzaForm)
        {
            if (pizzaForm.Pizza.UrlImage == null)
            {
                pizzaForm.Pizza.UrlImage = "/img/pizza_placeholder.png";
            }
            if (!ModelState.IsValid)
            {
                List<Category> categories = _pizzaDatabase.Categories.ToList();

                List<Ingredient> dbIngredients = _pizzaDatabase.Ingredients.ToList();

                List<SelectListItem> availableIngredients = new List<SelectListItem>();

                if (dbIngredients != null)
                {
                    foreach (Ingredient ingredient in dbIngredients)
                    {
                        string ingredientStringId = ingredient.Id.ToString();

                        availableIngredients.Add(new SelectListItem { Value = ingredientStringId, Text = ingredient.Name });
                    }
                }

                pizzaForm.Ingredients = availableIngredients;
                pizzaForm.Categories = categories;

                return View("Create", pizzaForm);
            }

            Pizza newPizza = new Pizza();
            newPizza.Name = pizzaForm.Pizza.Name;
            newPizza.Description = pizzaForm.Pizza.Description;
            newPizza.UrlImage = pizzaForm.Pizza.UrlImage;
            newPizza.Price = pizzaForm.Pizza.Price;

            if (pizzaForm.Pizza.CategoryId != null)
            {
                newPizza.CategoryId = pizzaForm.Pizza.CategoryId;
            }

            newPizza.Ingredients = new List<Ingredient>(); 

            if (pizzaForm.SelectedIngredients != null)
            {
                foreach(String ingredientId in pizzaForm.SelectedIngredients)
                {
                    long parsedIngredientId = long.Parse(ingredientId);

                    Ingredient? dbIngredient = _pizzaDatabase.Ingredients.Where(i => i.Id == parsedIngredientId).FirstOrDefault();

                    if(dbIngredient != null)
                    {
                        newPizza.Ingredients.Add(dbIngredient);
                    }
                }
            }

            _pizzaDatabase.Pizza.Add(newPizza);
            _pizzaDatabase.SaveChanges();

            _logger.WriteFileLog($"Aggiunta al database una pizza con id = {pizzaForm.Pizza.Id}");

            return RedirectToAction("Index", "Home");
            
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public IActionResult Update(long id)
        {
            
            Pizza? pizzaToEdit = _pizzaDatabase.Pizza.Where(p => p.Id == id).Include(pizza => pizza.Category).Include(pizza => pizza.Ingredients).FirstOrDefault();

            if(pizzaToEdit == null)
            {
                _logger.WriteFileLog($"Tentativo fallito di mostrare form di modifica della pizza con id = {id}");
                return NotFound();
            } else
            {
                List<Category> categories = _pizzaDatabase.Categories.ToList();
                List<Ingredient> dbIngredients = _pizzaDatabase.Ingredients.ToList();

                List<SelectListItem> availableIngredients = new List<SelectListItem>();

                if (dbIngredients != null)
                {
                    foreach (Ingredient ingredient in dbIngredients)
                    {
                        string ingredientStringId = ingredient.Id.ToString();

                        availableIngredients.Add(new SelectListItem { 
                            Value = ingredientStringId,
                            Text = ingredient.Name,
                            Selected = pizzaToEdit.Ingredients.Any(i => i.Id == ingredient.Id)
                        });
                    }
                }

                PizzaFormModel model = new PizzaFormModel();

                model.Ingredients = availableIngredients;
                model.Pizza = pizzaToEdit;
                model.Categories = categories;
                return View(model);
            }
            
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        [ValidateAntiForgeryToken]
        public IActionResult Update(long id, PizzaFormModel updatedPizzaModel)
        {
            if (updatedPizzaModel.Pizza.UrlImage == null)
            {
                updatedPizzaModel.Pizza.UrlImage = "/img/pizza_placeholder.png";
            }
            if (!ModelState.IsValid)
            {

                List<Category> categories = _pizzaDatabase.Categories.ToList();

                List<Ingredient> dbIngredients = _pizzaDatabase.Ingredients.ToList();

                List<SelectListItem> availableIngredients = new List<SelectListItem>();

                if (dbIngredients != null)
                {
                    foreach (Ingredient ingredient in dbIngredients)
                    {
                        string ingredientStringId = ingredient.Id.ToString();

                        availableIngredients.Add(new SelectListItem { Value = ingredientStringId, Text = ingredient.Name });
                    }
                }

                updatedPizzaModel.Categories = categories;
                updatedPizzaModel.Ingredients = availableIngredients;

                _logger.WriteFileLog($"Inserimento di dati errati durante il tentativo di modifica della pizza con id = {id}");
                return View("Update", updatedPizzaModel);
            }

            
            Pizza? previousPizza = _pizzaDatabase.Pizza.Where(p => p.Id == id).Include(pizza => pizza.Ingredients).FirstOrDefault();

            if(previousPizza != null)
            {
                previousPizza.Name = updatedPizzaModel.Pizza.Name;
                previousPizza.Description = updatedPizzaModel.Pizza.Description;
                previousPizza.UrlImage = updatedPizzaModel.Pizza.UrlImage;
                previousPizza.Price = updatedPizzaModel.Pizza.Price;
                previousPizza.CategoryId = updatedPizzaModel.Pizza.CategoryId;

                if (updatedPizzaModel.Pizza.CategoryId != null)
                {
                    previousPizza.CategoryId = updatedPizzaModel.Pizza.CategoryId;
                }

                previousPizza.Ingredients?.Clear();

                if (updatedPizzaModel.SelectedIngredients != null)
                {
                    foreach (String ingredientId in updatedPizzaModel.SelectedIngredients)
                    {
                        long parsedIngredientId = long.Parse(ingredientId);

                        Ingredient? dbIngredient = _pizzaDatabase.Ingredients.Where(i => i.Id == parsedIngredientId).FirstOrDefault();

                        if (dbIngredient != null)
                        {
                            previousPizza.Ingredients.Add(dbIngredient);
                        }
                    }
                }

                _pizzaDatabase.SaveChanges();

                _logger.WriteFileLog($"Modificata con successo la pizza con id = {id}");

                return RedirectToAction("Index", "Home");
            } else
            {
                _logger.WriteFileLog($"Tentativo fallito di modifica della pizza con id = {id}");
                return NotFound("Non è stata trovata la pizza da modificare.");
            }
           
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(long id)
        {
            
            Pizza? pizzaToDelete = _pizzaDatabase.Pizza.Where(p => p.Id == id).FirstOrDefault();

            if (pizzaToDelete != null)
            {
                _pizzaDatabase.Pizza.Remove(pizzaToDelete);
                _pizzaDatabase.SaveChanges();

                _logger.WriteFileLog($"Eliminata con successo la pizza con id = {id}");

                return RedirectToAction("Index", "Home");
            }
            else
            {
                _logger.WriteFileLog($"Tentativo fallito di eliminazione della pizza con id = {id}");
                return NotFound("La pizza da eliminare non è stata trovata.");
            }
            
        }

        [HttpGet]
        public IActionResult MenuApi()
        {
            return View("IndexApi", "Pizza");
        }

        [HttpGet]
        public IActionResult GetModalPizzaDetail(long id)
        {

            Pizza? foundPizza = _pizzaDatabase.Pizza.Where(p => p.Id == id).Include(pizza => pizza.Category).Include(pizza => pizza.Ingredients).FirstOrDefault();

            return PartialView("ModalDetail", foundPizza);
        }
    }
}
