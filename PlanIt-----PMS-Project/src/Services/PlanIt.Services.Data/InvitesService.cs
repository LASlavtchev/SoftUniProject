namespace PlanIt.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using PlanIt.Data.Common.Repositories;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class InvitesService : IInvitesService
    {
        private readonly IRepository<Invite> invitesRepository;

        public InvitesService(
            IRepository<Invite> invitesRepository)
        {
            this.invitesRepository = invitesRepository;
        }

        public string GenerateUniqueSecurityValue()
        {
            string uniqueInviteValue = Guid.NewGuid().ToString("N");

            return uniqueInviteValue;
        }

        public async Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>()
        {
            var all = await this.invitesRepository
                .All()
                .To<TViewModel>()
                .ToListAsync();

            return all;
        }

        public async Task<TViewModel> GetByEmailAsync<TViewModel>(string email)
        {
            var invite = await this.invitesRepository.All()
                .Where(i => i.Email == email)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            return invite;
        }

        public async Task<Invite> CreateInviteByUserAsync(string email, string purpose)
        {
            var invite = new Invite
            {
                Email = email,
                Purpose = purpose,
                IsInvited = false,
                RequestExpiredOn = DateTime.UtcNow.AddHours(24),
            };

            await this.invitesRepository.AddAsync(invite);
            var result = await this.invitesRepository.SaveChangesAsync();

            return invite;
        }
    }
}
