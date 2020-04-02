namespace PlanIt.Web.ViewModels.Users
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class UserAdminDeleteViewModel : IMapFrom<PlanItUser>
    {
        public string Id { get; set; }
    }
}
