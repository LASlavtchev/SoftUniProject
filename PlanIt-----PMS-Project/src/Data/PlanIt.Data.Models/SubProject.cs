namespace PlanIt.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    using PlanIt.Data.Common.Models;

    public class SubProject : BaseDeletableModel<int>
    {
        public SubProject()
        {
            this.AdditionalCosts = new HashSet<AdditionalCost>();
            this.Problems = new HashSet<Problem>();
        }

        public int ProjectId { get; set; }

        public Project Project { get; set; }

        public int SubProjectTypeId { get; set; }

        public SubProjectType SubProjectType { get; set; }

        // Set by ProjectManager for control - must be before Project DueDate
        public DateTime? DueDate { get; set; }

        // Set by ProjectManager
        [Column(TypeName = "decimal(15,4)")]
        public decimal Budget { get; set; }

        public int ProgressStatusId { get; set; }

        public virtual ProgressStatus ProgressStatus { get; set; }

        public virtual ICollection<AdditionalCost> AdditionalCosts { get; set; }

        public virtual ICollection<Problem> Problems { get; set; }
    }
}
