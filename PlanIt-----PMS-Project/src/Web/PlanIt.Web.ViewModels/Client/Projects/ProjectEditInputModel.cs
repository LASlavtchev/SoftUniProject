namespace PlanIt.Web.ViewModels.Client.Projects
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using PlanIt.Common;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;
    using PlanIt.Web.ViewModels.ValidationAttributes;

    public class ProjectEditInputModel : IMapFrom<Project>, IMapTo<Project>
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Project name")]
        public string Name { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950330", ErrorMessage = GlobalConstants.ErrorMessageNonNegativeNumber)]
        [DisplayName("Budget")]
        public decimal ClientBudget { get; set; }

        [DateTimeAfterNow(ErrorMessage = GlobalConstants.ErrorMessageDateTime)]
        [DisplayName("Expected due date")]
        public DateTime ClientDueDate { get; set; }
    }
}
