using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.ValidationAttributes
{
    public class MoreThanFourWords : ValidationAttribute
    {
        protected override ValidationResult? IsValid(Object? value, ValidationContext validationContext)
        {
            if (value is string)
            {
                string inputValue = (string)value;

                if (inputValue == null || inputValue.Split(" ").Length < 5)
                {
                    return new ValidationResult("Questo campo deve contenere almeno 5 parole.");
                }

                return ValidationResult.Success;

            }

            return new ValidationResult("Il valore inserito non è di tipo stringa.");
        }
    }
}
