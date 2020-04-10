namespace PlanIt.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PlanIt.Data.Models;

    public interface IProjectsService
    {
        int AllCount();

        Task<IEnumerable<TViewModel>> GetAllByClientIdAsync<TViewModel>(int clientId);

        Task<IEnumerable<TViewModel>> GetAllApprovedByClientIdAsync<TViewModel>(int clientId);

        Task<IEnumerable<TViewModel>> GetAllNotApprovedByClientIdAsync<TViewModel>(int clientId);

        Task<IEnumerable<TViewModel>> GetAllByManagerIdAsync<TViewModel>(string managerId);

        Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>();

        Task<TViewModel> GetByIdAsync<TViewModel>(int projectId);

        Task<Project> CreateByClientAsync<TInputModel>(TInputModel model);

        Task<Project> ApproveAsync(int projectId);

        Task DeleteAsync(int projectId);

        Task<Project> EditAsync<TInputModel>(TInputModel inputModel);
    }
}
