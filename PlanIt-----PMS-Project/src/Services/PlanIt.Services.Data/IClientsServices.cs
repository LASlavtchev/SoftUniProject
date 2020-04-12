namespace PlanIt.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PlanIt.Data.Models;

    public interface IClientsServices
    {
        int GetAllCount();

        Task<Client> CreateAsync(string userId);

        Task DeleteAsync(string userId);

        Task RestoreAsync(string userId);

        bool IsDeletedClientExist(string userId);

        Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>();

        Task<TViewModel> GetClientByUserIdAsync<TViewModel>(string userId);

        Task<TViewModel> GetClientByIdAsync<TViewModel>(int clientId);

        Task<int> GetClientIdByUserIdAsync(string userId);
    }
}
