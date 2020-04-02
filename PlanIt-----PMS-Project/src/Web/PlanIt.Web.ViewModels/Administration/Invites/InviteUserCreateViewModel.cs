namespace PlanIt.Web.ViewModels.Administration.Invites
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class InviteUserCreateViewModel : IMapFrom<Invite>
    {
        public string Email { get; set; }
    }
}
