namespace PlanIt.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PlanIt.Data.Models;

    public interface IProjectsService
    {
        int AllCount();

        int AllCountByClientId(int clientId);

        int AllApprovedCountByClientId(int clientId);

        int AllNotApprovedCountByClientId(int clientId);

        decimal CalculateApprovedProjectsBudgetByClientId(int clientId);

        Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>();

        Task<IEnumerable<TViewModel>> GetAllByClientIdAsync<TViewModel>(int clientId);

        Task<IEnumerable<TViewModel>> GetAllApprovedByClientIdAsync<TViewModel>(int clientId);

        Task<IEnumerable<TViewModel>> GetAllNotApprovedByClientIdAsync<TViewModel>(int clientId);

        Task<IEnumerable<TViewModel>> GetAllByManagerIdAsync<TViewModel>(string managerId);

        IEnumerable<Project> GetAllByManagerId(string managerId);

        Task<IEnumerable<TViewModel>> GetAllApprovedAsync<TViewModel>();

        Task<IEnumerable<TViewModel>> GetAllNotApprovedAsync<TViewModel>();

        Task<IEnumerable<TViewModel>> GetAllDeletedAsync<TViewModel>();

        Task<TViewModel> GetByIdAsync<TViewModel>(int projectId);

        Task<Project> GetByIdAsync(int projectId);

        Task<TViewModel> GetDeletedByIdAsync<TViewModel>(int projectId);

        Task<Project> CreateByClientAsync<TInputModel>(TInputModel model);

        Task<Project> CreateByManagerAsync<TInputModel>(TInputModel inputModel);

        Task<Project> ApproveAsync(int projectId);

        Task<Project> AssignManagerAsync(int projectId, string projectManagerId);

        Task DeleteAsync(int projectId);

        Task RestoreAsync(int projectId);

        Task<Project> EditByClientAsync<TInputModel>(TInputModel inputModel);

        Task<Project> EditByManagerAsync<TInputModel>(TInputModel inputModel);

        Task<Project> CalculateProjectBudget(int projectId);
    }
}
