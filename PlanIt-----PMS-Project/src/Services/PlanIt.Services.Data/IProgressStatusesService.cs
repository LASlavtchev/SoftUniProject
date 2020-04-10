namespace PlanIt.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProgressStatusesService
    {
        Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>();
    }
}
