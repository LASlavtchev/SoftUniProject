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

        Task HardDeleteAsync(string userId);

        Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>();
    }
}
