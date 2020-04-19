namespace PlanIt.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using PlanIt.Data.Common.Models;

    public class AdditionalCost : BaseDeletableModel<int>
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "decimal(15,4)")]
        public decimal PricePerQuantity { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(15,4)")]
        public decimal TotalCost { get; set; }

        public int? SubProjectId { get; set; }

        public SubProject SubProject { get; set; }

        public int ProjectId { get; set; }

        public Project Project { get; set; }
    }
}
