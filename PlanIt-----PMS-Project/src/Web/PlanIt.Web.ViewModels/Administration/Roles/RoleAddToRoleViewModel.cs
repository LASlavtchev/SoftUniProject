namespace PlanIt.Web.ViewModels.Administration.Roles
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class RoleAddToRoleViewModel : IMapFrom<PlanItRole>
    {
        public string Name { get; set; }
    }
}
