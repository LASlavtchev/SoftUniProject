namespace PlanIt.Web.ViewModels.Management.Projects
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using PlanIt.Common;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;
    using PlanIt.Web.ViewModels.ValidationAttributes;

    public class ProjectCreateInputModel : IMapTo<Project>
    {
        [Required]
        [DisplayName("Project name")]
        public string Name { get; set; }

        [DateTimeAfterNow(ErrorMessage = GlobalConstants.ErrorMessageDateTime)]
        [DisplayName("Start date")]
        public DateTime StartDate { get; set; }

        [DateTimeAfterDateTime("StartDate")]
        [DisplayName("Due date")]
        public DateTime DueDate { get; set; }

        [DisplayName("Client")]
        public int ClientId { get; set; }
    }
}
