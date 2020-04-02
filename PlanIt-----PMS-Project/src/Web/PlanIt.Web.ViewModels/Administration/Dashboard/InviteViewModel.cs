namespace PlanIt.Web.ViewModels.Administration.Dashboard
{
    using System;

    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class InviteViewModel : IMapFrom<Invite>
    {
        public bool IsInvited { get; set; }

        public DateTime? ExpiredOn { get; set; }

        public DateTime RequestExpiredOn { get; set; }
    }
}
