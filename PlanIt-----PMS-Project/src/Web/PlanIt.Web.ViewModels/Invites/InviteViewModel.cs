namespace PlanIt.Web.ViewModels.Invites
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;
    using System;

    public class InviteViewModel : IMapFrom<Invite>
    {
        public string Email { get; set; }

        public string Purpose { get; set; }

        public string SecurityValue { get; set; }

        public bool IsInvited { get; set; }

        public DateTime? ExpiredOn { get; set; }

        public DateTime RequestExpiredOn { get; set; }
    }
}
