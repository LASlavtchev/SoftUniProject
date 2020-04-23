namespace PlanIt.Web.ViewModels.AdditionalCosts.Projects
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;
    using PlanIt.Web.ViewModels.AdditionalCosts.SubProjects;

    public class AddCostCostsByProjectProjectViewModel : IMapFrom<Project>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<AddCostCostsByProjectSubProjectViewModel> SubProjects { get; set; }

        public List<AddCostCostsByProjectViewModel> AdditionalCostsToProjectOnly { get; set; }

        public decimal AdditionalCostsToProjectOnlyTotalCost =>
            this.AdditionalCostsToProjectOnly.Select(c => c.TotalCost).Sum();

        public decimal TotalCost { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Project, AddCostCostsByProjectProjectViewModel>()
                .ForMember(x => x.AdditionalCostsToProjectOnly, options =>
                {
                    options.MapFrom(p => p.AdditionalCosts.Where(ac => ac.SubProject.SubProjectType.Name == null));
                })
                .ForMember(x => x.TotalCost, options =>
                {
                    options.MapFrom(p => p.AdditionalCosts.Select(ac => ac.TotalCost).Sum());
                });
        }
    }
}
