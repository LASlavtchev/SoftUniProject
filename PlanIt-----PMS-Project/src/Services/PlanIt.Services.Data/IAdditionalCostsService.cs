namespace PlanIt.Services.Data
{
    using System.Threading.Tasks;

    using PlanIt.Data.Models;

    public interface IAdditionalCostsService
    {
        decimal SumAdditionalCostsCompletedProjectsByManagerId(string userId);

        Task<AdditionalCost> AddAsync<TInputModel>(TInputModel inputModel);

        Task<TViewModel> GetByIdAsync<TViewModel>(int costId);

        Task<AdditionalCost> EditAsync<TInputModel>(TInputModel inputModel);

        Task DeleteAsync(int costId);
    }
}
