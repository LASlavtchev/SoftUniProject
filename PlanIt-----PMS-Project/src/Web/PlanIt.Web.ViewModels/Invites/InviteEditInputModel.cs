namespace PlanIt.Web.ViewModels.Invites
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using PlanIt.Common;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;
    using PlanIt.Web.ViewModels.ValidationAttributes;

    public class InviteEditInputModel : IMapFrom<Invite>, IMapTo<Invite>
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = GlobalConstants.EmailAddressErrorMessage)]
        public string Email { get; set; }

        public bool IsInvited { get; set; }

        [DateTimeAfterNow(ErrorMessage = GlobalConstants.ErrorMessageDateTime)]
        public DateTime? ExpiredOn { get; set; }

        [DateTimeAfterNow(ErrorMessage = GlobalConstants.ErrorMessageDateTime)]
        public DateTime RequestExpiredOn { get; set; }
    }
}
