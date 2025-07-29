using System;
using System.ComponentModel.DataAnnotations;

namespace BankLoanSystem.Core.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NonNegativeAttribute : ValidationAttribute
    {
        public NonNegativeAttribute(string errorMessage = "Value must be non-negative.") : base(errorMessage)
        {
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success; // Let [Required] handle null cases
            }

            switch (value)
            {
                case int intValue when intValue < 0:
                case decimal decimalValue when decimalValue < 0:
                case double doubleValue when doubleValue < 0:
                    return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
