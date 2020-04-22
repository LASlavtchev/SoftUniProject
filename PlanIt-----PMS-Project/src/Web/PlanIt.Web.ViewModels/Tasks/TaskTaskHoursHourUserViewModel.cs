namespace PlanIt.Web.ViewModels.Tasks
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class TaskTaskHoursHourUserViewModel : IMapFrom<PlanItUser>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{this.FirstName} {this.LastName}";
    }
}
