namespace PlanIt.Web.ViewModels.AdditionalCosts
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using PlanIt.Common;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class AddCostAddToProjectInputModel : IMapTo<AdditionalCost>
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950330", ErrorMessage = GlobalConstants.ErrorMessageNonNegativeNumber)]
        [DisplayName("Price per")]
        public decimal PricePerQuantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = GlobalConstants.ErrorMessageNonNegativeNumber)]
        public int Quantity { get; set; }

        public int? SubProjectId { get; set; }

        public int ProjectId { get; set; }
    }
}
