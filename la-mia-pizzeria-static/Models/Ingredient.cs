using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.Models
{
    public class Ingredient
    {
        public long Id {  get; set; }
        [Required(ErrorMessage = "Il campo Nome è obbligatorio")]
        [MaxLength(50, ErrorMessage = "Il campo Nome può contenere al massimo 50 caratteri.")]
        public string Name { get; set; }
        [MaxLength(200, ErrorMessage = "Il campo Descrizione può contenere al massimo 200 caratteri.")]
        public string? Description { get; set; }

        // Relazione con Pizza
        public List<Pizza> Pizzas { get; set; }

        public Ingredient() { }
    }
}
