namespace PlanIt.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PlanIt.Data.Models;

    public interface IInvitesService
    {
        Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>();

        string GenerateUniqueSecurityValue();

        // Task<Invite> CreateInviteAsync<TViewModel>(TViewModel model);
    }
}
