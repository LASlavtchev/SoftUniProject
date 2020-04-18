namespace PlanIt.Web.ViewModels.SubProjects
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using PlanIt.Common;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;
    using PlanIt.Web.ViewModels.ValidationAttributes;

    public class SubProjectAddInputModel : IMapFrom<SubProject>, IMapTo<SubProject>
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Type")]
        public int SubProjectTypeId { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950330", ErrorMessage = GlobalConstants.ErrorMessageNonNegativeNumber)]
        public decimal Budget { get; set; }

        [DisplayName("Project")]
        public int ProjectId { get; set; }

        [DateTimeAfterNow(ErrorMessage = GlobalConstants.ErrorMessageDateTime)]
        [DisplayName("Due Date")]
        public DateTime? DueDate { get; set; }
    }
}
