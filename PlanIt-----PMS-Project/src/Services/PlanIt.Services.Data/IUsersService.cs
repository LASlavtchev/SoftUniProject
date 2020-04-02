namespace PlanIt.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    public interface IUsersService
    {
        Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>(ClaimsPrincipal user);

        Task<IEnumerable<TViewModel>> GetAllDeletedAsync<TViewModel>();

        Task<TViewModel> GetByIdAsync<TViewModel>(string id);

        Task<TViewModel> GetDeletedByIdAsync<TViewModel>(string id);

        Task RestoreAsync(string id);

        Task SoftDeleteAsync(string id);

        Task HardDeleteAsync(string id);
    }
}
