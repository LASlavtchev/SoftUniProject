namespace PlanIt.Services.Data
{
    using System.Threading.Tasks;

    using PlanIt.Data.Models;

    public interface IHoursService
    {
        Task<Hour> AddHour(Problem problem, PlanItUser user, decimal hours);
    }
}
