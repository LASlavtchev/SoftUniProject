namespace PlanIt.Web.ViewModels.Invites
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class InviteApproveInputModel : IMapFrom<Invite>, IMapTo<Invite>
    {
        public int Id { get; set; }
    }
}
