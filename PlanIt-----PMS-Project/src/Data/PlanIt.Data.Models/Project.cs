namespace PlanIt.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using PlanIt.Data.Common.Models;

    public class Project : BaseDeletableModel<int>
    {
        public Project()
        {
            this.SubProjects = new HashSet<SubProject>();
            this.AdditionalCosts = new HashSet<AdditionalCost>();
        }

        [Required]
        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? DueDate { get; set; }

        [Column(TypeName = "decimal(15,4)")]
        public decimal Budget { get; set; }

        public bool IsBudgetApproved { get; set; }

        public int ProgressStatusId { get; set; }

        public virtual ProgressStatus ProgressStatus { get; set; }

        public string ProjectManagerId { get; set; }

        public virtual PlanItUser ProjectManager { get; set; }

        public int ClientId { get; set; }

        public virtual Client Client { get; set; }

        [Column(TypeName = "decimal(15,4)")]
        public decimal ClientBudget { get; set; }

        public DateTime ClientDueDate { get; set; }

        public virtual ICollection<SubProject> SubProjects { get; set; }

        public virtual ICollection<AdditionalCost> AdditionalCosts { get; set; }
    }
}
