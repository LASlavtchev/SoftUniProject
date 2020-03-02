namespace PlanIt.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using PlanIt.Data.Common.Repositories;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class InvitesService : IInvitesService
    {
        private readonly IRepository<Invite> invitesRepository;

        public InvitesService(IRepository<Invite> invitesRepository)
        {
            this.invitesRepository = invitesRepository;
        }

        public IEnumerable<T> GetAll<T>()
        {
            var all = this.invitesRepository.All().To<T>().ToList();

            return all;
        }
    }
}
