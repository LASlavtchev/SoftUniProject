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

        Task<Problem> GetByIdAsync(int problemId);

        Task<TViewModel> GetByIdAsync<TViewModel>(int problemId);

        Task<Problem> AddHoursAsync(int problemId, decimal hours);

        Task<Problem> AddUserAsync(int problemId, PlanItUser user);

        Task<Problem> ChangeStatusAsync(int problemId, ProgressStatus progressStatus);

        Task<Problem> EditAsync<TInputModel>(TInputModel inputModel);
    }
}
