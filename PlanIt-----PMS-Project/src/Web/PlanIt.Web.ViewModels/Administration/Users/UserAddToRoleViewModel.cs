namespace PlanIt.Web.ViewModels.Administration.Users
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class UserAddToRoleViewModel : IMapFrom<PlanItUser>
    {
        public string Id { get; set; }

        public string Email { get; set; }
    }
}
