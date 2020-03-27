namespace PlanIt.Web.ViewModels.Invites
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class InviteDeleteViewModel : IMapFrom<Invite>
    {
        public int Id { get; set; }

        public string Email { get; set; }
    }
}
