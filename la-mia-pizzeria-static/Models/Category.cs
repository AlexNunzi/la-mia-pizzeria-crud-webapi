using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.Models
{
    public class Category
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Il campo Nome è obbligatorio")]
        [MaxLength(50, ErrorMessage = "Il campo Nome può contenere al massimo 50 caratteri.")]
        public string Name { get; set; }
        public List<Pizza> Pizzas { get; set; }

        public Category() { }
    }
}
