namespace PlanIt.Web.ViewModels.Dashboard
{
    using PlanIt.Web.ViewModels.Administration.Users;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class IndexAdministrationViewModel
    {
        // Invites
        public int InvitesCount { get; set; }

        public int InvitesApprovedCount { get; set; }

        public int InvitesInvitedExpiredOnCount { get; set; }

        public int InvitesRequestExpiredOnCount { get; set; }

        // Users
        public int UsersWithDeletedCount { get; set; }

        public int UsersCount { get; set; }

        public int UsersDeletedCount { get; set; }

        public int ClientsCount { get; set; }

        public int SettingsCount { get; set; }
    }
}
