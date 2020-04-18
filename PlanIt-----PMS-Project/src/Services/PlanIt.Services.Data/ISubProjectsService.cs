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
    }
}
