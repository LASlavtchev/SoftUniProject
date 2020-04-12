namespace PlanIt.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PlanIt.Data.Models;

    public interface IProgressStatusesService
    {
        Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>();

        Task<ProgressStatus> GetByNameAsync(string progressStatusName);
    }
}
