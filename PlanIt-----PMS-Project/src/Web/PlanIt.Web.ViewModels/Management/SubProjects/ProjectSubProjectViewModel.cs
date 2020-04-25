namespace PlanIt.Web.ViewModels.Management.SubProjects
{
    using System;
    using System.Linq;

    using AutoMapper;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class ProjectSubProjectViewModel : IMapFrom<SubProject>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string SubProjectTypeName { get; set; }

        public decimal Budget { get; set; }

        public string ProgressStatusName { get; set; }

        public DateTime? DueDate { get; set; }

        public decimal TotalSubProjectCosts { get; set; }

        public decimal TotalAdditionalCosts { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<SubProject, ProjectSubProjectViewModel>()
                .ForMember(x => x.TotalSubProjectCosts, options =>
                {
                    options.MapFrom(sp => sp.Problems.Select(p => p.TotalHours * p.HourlyRate).Sum());
                })
                .ForMember(x => x.TotalAdditionalCosts, options =>
                {
                    options.MapFrom(sp => sp.AdditionalCosts.Select(ac => ac.TotalCost).Sum());
                });
        }
    }
}
