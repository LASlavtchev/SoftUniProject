namespace PlanIt.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IInvitesService
    {
        IEnumerable<T> GetAll<T>();
    }
}
