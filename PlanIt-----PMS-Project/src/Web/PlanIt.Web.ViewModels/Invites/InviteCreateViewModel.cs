namespace PlanIt.Web.ViewModels.Invites
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using PlanIt.Common;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;
    using PlanIt.Web.ViewModels.ValidationAttributes;

    public class InviteCreateViewModel : IMapTo<Invite>
    {
        [Required]
        [EmailAddress(ErrorMessage = GlobalConstants.EmailAddressErrorMessage)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [DateTimeAfterNow]
        public DateTime ExpiredOn { get; set; }
    }
}
