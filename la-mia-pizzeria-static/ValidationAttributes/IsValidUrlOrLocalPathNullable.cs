using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.ValidationAttributes
{
    public class IsValidUrlOrLocalPathNullable : ValidationAttribute
    {
        protected override ValidationResult? IsValid(Object? value, ValidationContext validationContext)
        {

            if(value is string)
            {
                string userInput = (string)value;
                if ((userInput.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                    || userInput.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
                    || userInput.StartsWith("/img/", StringComparison.OrdinalIgnoreCase)))
                {
                    return ValidationResult.Success;
                }
            } else if (value is null) 
            {
                return ValidationResult.Success;
            }
            
            return new ValidationResult("Questo campo deve contenere un URL valido o una path ad una immagine locale.");
        }
    }
}
