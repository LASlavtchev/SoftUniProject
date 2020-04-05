namespace PlanIt.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    public interface IUsersService
    {
        int GetAllWithDeletedCount();

        int GetAllCount();

        int GetAllDeletedCount();

        Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>(ClaimsPrincipal user);

        Task<IEnumerable<TViewModel>> GetAllDeletedAsync<TViewModel>();

        Task<TViewModel> GetDeletedByIdAsync<TViewModel>(string id);

        Task RestoreAsync(string id);

        Task DeleteAsync(string id);
    }
}
