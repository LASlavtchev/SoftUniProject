namespace PlanIt.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PlanIt.Data.Models;

    public interface IInvitesService
    {
        int GetAllCount();

        int GetAllApprovedCount();

        int GetAllInvitedExpiredOnCount();

        int GetAllRequestExpiredOnCount();

        Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>();

        Task<TViewModel> GetByEmailAsync<TViewModel>(string email);

        Task<TViewModel> GetByIdAsync<TViewModel>(int? id);

        Task<Invite> CreateAsync<TInputModel>(TInputModel model);

        string GenerateUniqueSecurityValue();

        Task<Invite> CreateInviteByUserAsync(string email, string purpose);

        Task<Invite> ApproveAsync(int id);

        Task<Invite> ExtendAsync<TInputModel>(TInputModel model);

        Task DeleteAsync(int? id);
    }
}
