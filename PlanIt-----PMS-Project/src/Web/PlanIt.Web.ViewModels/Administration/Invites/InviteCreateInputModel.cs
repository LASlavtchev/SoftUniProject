namespace PlanIt.Web.ViewModels.Administration.Invites
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using PlanIt.Common;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;
    using PlanIt.Web.ViewModels.ValidationAttributes;

    public class InviteCreateInputModel : IMapTo<Invite>, IMapFrom<Invite>
    {
        [Required]
        [EmailAddress(ErrorMessage = GlobalConstants.EmailAddressErrorMessage)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [DateTimeAfterNow(ErrorMessage = GlobalConstants.ErrorMessageDateTime)]
        public DateTime ExpiredOn { get; set; }

        [Required]
        [StringLength(
            GlobalConstants.MaxLengthString,
            ErrorMessage = GlobalConstants.ErrorMessageStringLength,
            MinimumLength = GlobalConstants.MinLengthString)]
        public string Purpose { get; set; }
    }
}
