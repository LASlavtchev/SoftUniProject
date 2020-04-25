namespace PlanIt.Web.ViewModels.SubProjects
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using PlanIt.Common;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;
    using PlanIt.Web.ViewModels.ValidationAttributes;

    public class SubProjectEditInputModel : IMapFrom<SubProject>, IMapTo<SubProject>
    {
        public int Id { get; set; }

        [DisplayName("Type")]
        public string SubProjectTypeName { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950330", ErrorMessage = GlobalConstants.ErrorMessageNonNegativeNumber)]
        public decimal Budget { get; set; }

        [DisplayName("Status")]
        public int ProgressStatusId { get; set; }

        public int ProjectId { get; set; }

        [DisplayName("Project Name")]
        public string ProjectName { get; set; }

        [Required]
        [DisplayName("Due Date")]
        public DateTime? DueDate { get; set; }
    }
}
