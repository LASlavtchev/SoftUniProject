namespace PlanIt.Web.ViewModels.Invites
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class InviteRegisterViewModel : IMapFrom<Invite>
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string SecurityValue { get; set; }

        public bool IsInvited { get; set; }
    }
}
