namespace PlanIt.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using PlanIt.Data.Common.Models;

    public class SubProjectType : BaseDeletableModel<int>
    {
        public SubProjectType()
        {
            this.SubProjects = new HashSet<SubProject>();
        }

        [Required]
        public string Name { get; set; }

        public ICollection<SubProject> SubProjects { get; set; }
    }
}
