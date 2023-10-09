using la_mia_pizzeria_static.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace la_mia_pizzeria_static.Models
{
    public class Pizza
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Il campo Nome è obbligatorio")]
        [MaxLength(50, ErrorMessage = "Il campo Nome può contenere al massimo 50 caratteri.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Il campo Descrizione è obbligatorio")]
        [MoreThanFourWords]
        [MaxLength(1000, ErrorMessage = "Il campo Descrizione può contenere al massimo 1000 caratteri.")]
        public string Description { get; set; }
        [IsValidUrlOrLocalPathNullable(ErrorMessage = "Questo campo deve contenere un URL valido ad un'immagine.")]
        [MaxLength(500, ErrorMessage = "L'URL non può essere lungo più di 500 caratteri.")]
        public string? UrlImage { get; set; }
        [Range(1, 1000, ErrorMessage = "Il prezzo deve essere un numero compreso tra 1 e 1000.")]
        public float Price { get; set; }

        // Relazione con Category
        public long? CategoryId {  get; set; }
        public Category? Category { get; set; }

        // Relazione con Ingredient
        public List<Ingredient>? Ingredients { get; set; }

        public Pizza() { }

        public Pizza(string name, string description, string urlImage, float price)
        {
            this.Name = name;
            this.Description = description;
            this.UrlImage = urlImage;
            this.Price = price;
        }
    }
}
