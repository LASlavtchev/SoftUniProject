namespace PlanIt.Web.ViewModels.Tasks.Hours
{
    using System;

    using AutoMapper;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class TaskTaskHoursHourViewModel : IMapFrom<Hour>, IHaveCustomMappings
    {
        public DateTime Date { get; set; }

        public decimal WorkedHours { get; set; }

        public string UserFullName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Hour, TaskTaskHoursHourViewModel>()
                .ForMember(x => x.UserFullName, options =>
                {
                    options.MapFrom(h => $"{h.User.FirstName} {h.User.LastName}");
                });
        }
    }
}
