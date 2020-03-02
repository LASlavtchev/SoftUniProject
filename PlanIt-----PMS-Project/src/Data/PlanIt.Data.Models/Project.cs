namespace PlanIt.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using PlanIt.Data.Common.Models;

    public class Project : BaseDeletableModel<int>
    {
        public Project()
        {
            this.UserProjects = new HashSet<UserProject>();
        }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<UserProject> UserProjects { get; set; }
    }
}
