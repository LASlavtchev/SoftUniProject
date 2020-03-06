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
        public decimal WorkedTime { get; set; }

        public bool IsCompleted { get; set; }

        public string PlantItUserId { get; set; }

        public PlanItUser Contractor { get; set; }

        public int SubProjectId { get; set; }

        public SubProject SubProject { get; set; }
    }
}
