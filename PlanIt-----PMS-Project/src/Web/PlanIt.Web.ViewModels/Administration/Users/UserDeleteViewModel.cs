namespace PlanIt.Web.ViewModels.Administration.Users
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class UserDeleteViewModel : IMapFrom<PlanItUser>
    {
        public string Id { get; set; }
    }
}
