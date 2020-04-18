namespace PlanIt.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using PlanIt.Data.Models;

    public interface ISubProjectsService
    {
        Task<SubProject> CreateAsync<TInputModel>(TInputModel inputModel);

        Task<TInputModel> GetByIdAsync<TInputModel>(int subProjectId);

        Task<SubProject> EditAsync<TInputModel>(TInputModel inputModel);

        Task DeleteAsync(int subProjectId);
    }
}
