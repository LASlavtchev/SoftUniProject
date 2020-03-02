namespace PlanIt.Web.ViewModels.Invites
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class InviteViewModel : IMapFrom<Invite>
    {
        public string Email { get; set; }

        public string SecurityValue { get; set; }

        public string CreatedOn { get; set; }

        public string ModifiedOn { get; set; }

        public string ExpiredOn { get; set; }
    }
}
