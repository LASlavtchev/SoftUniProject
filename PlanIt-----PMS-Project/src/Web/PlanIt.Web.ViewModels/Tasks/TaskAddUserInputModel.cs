namespace PlanIt.Web.ViewModels.Tasks
{
    using System.ComponentModel.DataAnnotations;

    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class TaskAddUserInputModel : IMapFrom<Problem>
    {
        public int Id { get; set; }

        public string SubProjectProjectName { get; set; }

        public string SubProjectSubProjectTypeName { get; set; }

        public string Name { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
