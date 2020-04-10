namespace PlanIt.Web.ViewModels.ValidationAttributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class DateTimeAfterDateTimeAttribute : ValidationAttribute
    {
        private const string CurrentDateNotValid = "The current date is not valid!";
        private const string ExceptionMessage = "The current date is not after the previous date!";

        private readonly string otherPropertyName;

        public DateTimeAfterDateTimeAttribute(string otherPropertyname)
        {
            this.otherPropertyName = otherPropertyname;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var currentDateTime = (DateTime?)value;

            var otherValue = validationContext
                .ObjectType
                .GetProperty(this.otherPropertyName)
                .GetValue(validationContext.ObjectInstance);
            var dateTimeBefore = (DateTime?)otherValue;

            if (otherValue != null && dateTimeBefore == null)
            {
                return new ValidationResult(CurrentDateNotValid);
            }

            if (dateTimeBefore >= currentDateTime)
            {
                return new ValidationResult(ExceptionMessage);
            }

            return ValidationResult.Success;
        }
    }
}
