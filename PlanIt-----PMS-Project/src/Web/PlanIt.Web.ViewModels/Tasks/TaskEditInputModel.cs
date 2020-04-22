namespace PlanIt.Web.ViewModels.Tasks
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using PlanIt.Common;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class TaskEditInputModel : IMapFrom<Problem>, IMapTo<Problem>
    {
        public int Id { get; set; }

        public int SubProjectId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Instructions { get; set; }

        public int ProgressStatusId { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950330", ErrorMessage = GlobalConstants.ErrorMessageNonNegativeNumber)]
        [DisplayName("Hourly rate")]
        public decimal HourlyRate { get; set; }

        [DisplayName("Due date")]
        public DateTime? DueDate { get; set; }
    }
}
