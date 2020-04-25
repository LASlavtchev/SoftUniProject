namespace PlanIt.Services.Data
{
    using System.Threading.Tasks;

    using PlanIt.Data.Models;

    public interface ISubProjectsService
    {
        Task<SubProject> CreateAsync<TInputModel>(TInputModel inputModel);

        Task<TViewModel> GetByIdAsync<TViewModel>(int subProjectId);

        Task<SubProject> EditAsync<TInputModel>(TInputModel inputModel);

        Task DeleteAsync(int subProjectId);

        Task<SubProject> ChangeStatusAsync(int subProjectId, ProgressStatus progressStatus);
    }
}
