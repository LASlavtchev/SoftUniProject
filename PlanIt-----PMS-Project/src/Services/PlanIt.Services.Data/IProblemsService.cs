namespace PlanIt.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using PlanIt.Data.Models;

    public interface IProblemsService
    {
        Task<Problem> AssignAsync<TInputModel>(PlanItUser user, TInputModel model);

        Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>();

        Task<IEnumerable<TViewModel>> GetAllByUserIdAsync<TViewModel>(string userId);

        Task<IEnumerable<TViewModel>> GetAllByManagerIdAsync<TViewModel>(string userId);

        Task<Problem> GetByIdAsync(int taskId);

        Task<TViewModel> GetByIdAsync<TViewModel>(int taskId);

        Task<Problem> AddHoursAsync(int taskId, decimal hours);

        Task<Problem> AddUserAsync(int taskId, PlanItUser user);

        Task<Problem> ChangeStatusAsync(int taskId, ProgressStatus progressStatus);
    }
}
