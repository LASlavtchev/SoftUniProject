namespace PlanIt.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    using PlanIt.Data.Common.Models;

    // Tracking the labour hours
    public class Hour : BaseDeletableModel<int>
    {
        // The current date that the hours are made
        public DateTime Date { get; set; }

        [Column(TypeName = "decimal(15,4)")]
        public decimal WorkedHours { get; set; }

        public string UserId { get; set; }

        public PlanItUser User { get; set; }

        public int ProblemId { get; set; }

        public Problem Problem { get; set; }
    }
}
