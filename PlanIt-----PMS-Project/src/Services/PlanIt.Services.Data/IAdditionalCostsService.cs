namespace PlanIt.Services.Data
{
    using PlanIt.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IAdditionalCostsService
    {
        Task<AdditionalCost> AddAsync<TInputModel>(TInputModel inputModel);
    }
}
