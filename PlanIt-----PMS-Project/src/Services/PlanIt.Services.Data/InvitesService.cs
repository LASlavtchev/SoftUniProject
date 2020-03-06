namespace PlanIt.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using PlanIt.Data.Common.Repositories;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class InvitesService : IInvitesService
    {
        private readonly IRepository<Invite> invitesRepository;
        private readonly UserManager<PlanItUser> userManager;
        // private readonly IMapper mapper;

        public InvitesService(
            IRepository<Invite> invitesRepository,
            UserManager<PlanItUser> userManager)
        {
            this.invitesRepository = invitesRepository;
            this.userManager = userManager;
            // this.mapper = mapper;
        }

        public string GenerateUniqueSecurityValue()
        {
            string uniqueInviteValue = Guid.NewGuid().ToString("N");

            return uniqueInviteValue;
        }

        public async Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>()
        {
            var all = await this.invitesRepository.All().To<TViewModel>().ToListAsync();

            return all;
        }

        //public async Task<Invite> CreateInviteAsync<TViewModel>(TViewModel model)
        //{
        //    var invite = this.mapper.Map<Invite>(model);

        //    await this.invitesRepository.AddAsync(invite);

        //    return invite;
        //}
    }
}
