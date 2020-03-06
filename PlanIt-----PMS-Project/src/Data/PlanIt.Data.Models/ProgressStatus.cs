namespace PlanIt.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using PlanIt.Data.Common.Models;

    public class ProgressStatus : BaseDeletableModel<int>
    {
        public ProgressStatus()
        {
            this.Problems = new HashSet<Problem>();
            this.SubProjects = new HashSet<SubProject>();
            this.Projects = new HashSet<Project>();
        }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Problem> Problems { get; set; }

        public virtual ICollection<SubProject> SubProjects { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
    }
}
