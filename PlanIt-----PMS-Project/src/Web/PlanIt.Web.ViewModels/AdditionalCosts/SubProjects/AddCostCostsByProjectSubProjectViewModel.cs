namespace PlanIt.Web.ViewModels.AdditionalCosts.SubProjects
{
    using System.Linq;

    using AutoMapper;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class AddCostCostsByProjectSubProjectViewModel : IMapFrom<SubProject>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string SubProjectTypeName { get; set; }

        public decimal AdditionalCostsSum { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<SubProject, AddCostCostsByProjectSubProjectViewModel>()
                .ForMember(x => x.AdditionalCostsSum, options =>
                {
                    options.MapFrom(sp => sp.AdditionalCosts.Select(ac => ac.TotalCost).Sum());
                });
        }
    }
}
