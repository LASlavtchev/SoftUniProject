namespace PlanIt.Web.ViewModels.Management.Clients
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class ProjectClientViewModel : IMapFrom<Client>
    {
        public int Id { get; set; }

        public string PlantItUserId { get; set; }

        public string PlantItUserFirstName { get; set; }

        public string PlantItUserMiddleName { get; set; }

        public string PlantItUserLastName { get; set; }

        public string FullName =>
            $"{this.PlantItUserFirstName} {this.PlantItUserMiddleName} {this.PlantItUserLastName}";

        public string PlantItUserCompanyName { get; set; }

        public string PlantItUserJobTitle { get; set; }

        public string PlantItUserEmail { get; set; }

        public string PlantItUserPhoneNumber { get; set; }
    }
}
