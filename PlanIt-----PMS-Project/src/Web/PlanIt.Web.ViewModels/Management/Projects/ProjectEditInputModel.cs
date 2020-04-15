namespace PlanIt.Web.ViewModels.Management.Projects
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

        [DateTimeAfterNow(ErrorMessage = GlobalConstants.ErrorMessageDateTime)]
        [DisplayName("Start date")]
        public DateTime StartDate { get; set; }

        [DateTimeAfterDateTime("StartDate")]
        [DisplayName("Due date")]
        public DateTime DueDate { get; set; }

        [DisplayName("Progress status")]
        public int ProgressStatusId { get; set; }

        [DisplayName("Project manager")]
        public string ProjectManagerId { get; set; }
    }
}
