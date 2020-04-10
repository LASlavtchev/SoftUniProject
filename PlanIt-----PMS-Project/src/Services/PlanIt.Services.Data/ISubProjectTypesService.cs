namespace PlanIt.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISubProjectTypesService
    {
        Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>();
    }
}
