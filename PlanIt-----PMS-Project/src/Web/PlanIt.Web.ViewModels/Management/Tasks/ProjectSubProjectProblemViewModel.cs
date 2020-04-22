namespace PlanIt.Web.ViewModels.Management.Tasks
{
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class ProjectSubProjectProblemViewModel : IMapFrom<Problem>
    {
        public decimal TotalHours { get; set; }

        public decimal HourlyRate { get; set; }

        public decimal Cost => this.TotalHours * this.HourlyRate;
    }
}
