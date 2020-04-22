namespace PlanIt.Web.ViewModels.Tasks
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class TaskIndexUserViewModel : IMapFrom<PlanItUser>
    {
        public string Email { get; set; }

        public string Phonenumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{this.FirstName} {this.LastName}";
    }
}
