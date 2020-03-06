namespace PlanIt.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using PlanIt.Data.Common.Models;

    public class Problem : BaseDeletableModel<int>
    {
        public Problem()
        {
            this.UserProblems = new HashSet<UserProblem>();
            this.Hours = new HashSet<Hour>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Instructions { get; set; }

        // Set by SubProjectManager for control - must be before Subproject DueDate, Project DueDate
        public DateTime? DueDate { get; set; }

        [Column(TypeName = "decimal(15,4)")]
        public decimal TotalHours { get; set; }

        [Column(TypeName = "decimal(15,4)")]
        public decimal HourlyRate { get; set; }

        public int ProgressStatusId { get; set; }

        public virtual ProgressStatus ProgressStatus { get; set; }

        public int SubProjectId { get; set; }

        public virtual SubProject SubProject { get; set; }

        public virtual ICollection<UserProblem> UserProblems { get; set; }

        public virtual ICollection<Hour> Hours { get; set; }
    }
}
