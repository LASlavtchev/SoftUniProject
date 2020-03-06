namespace PlanIt.Web.ViewModels.ValidationAttributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class DateTimeAfterNowAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var dateTimeNow = DateTime.UtcNow;
            var dateTime = (DateTime)value;

            if (dateTime == null)
            {
                return false;
            }

            return dateTime > dateTimeNow;
        }
    }
}
