namespace PlanIt.Web.ViewModels.Tasks
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using PlanIt.Common;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class TaskAssignInputModel : IMapFrom<Problem>, IMapTo<Problem>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Instructions { get; set; }

        public int SubProjectId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950330", ErrorMessage = GlobalConstants.ErrorMessageNonNegativeNumber)]
        [DisplayName("Hourly rate")]
        public decimal HourlyRate { get; set; }

        [DisplayName("Due date")]
        public DateTime? DueDate { get; set; }

        public int ProjectId { get; set; }
    }
}
