namespace PlanIt.Web.ViewModels.Administration.Dashboard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class IndexViewModel
    {
        public IEnumerable<InviteViewModel> Invites { get; set; }

        public int InvitesCount => this.Invites.Count();

        public int InvitesApprovedCount => this.Invites
            .Where(i => i.IsInvited && i.ExpiredOn > DateTime.UtcNow)
            .Count();

        public int InvitesInviteExpiredOnCount => this.Invites
            .Where(i => i.IsInvited && i.ExpiredOn <= DateTime.UtcNow)
            .Count();

        public int InvitesRequestExpiredOnCount => this.Invites
            .Where(i => !i.IsInvited && i.RequestExpiredOn <= DateTime.UtcNow)
            .Count();

        public int SettingsCount { get; set; }
    }
}
