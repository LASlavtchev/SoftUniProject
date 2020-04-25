namespace PlanIt.Web.ViewModels.Dashboard
{
    public class IndexManagementViewModel
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
