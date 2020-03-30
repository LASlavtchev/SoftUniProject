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
    using PlanIt.Services.Messaging;

    public class InvitesService : IInvitesService
    {
        private readonly IRepository<Invite> invitesRepository;
        private readonly IEmailSender emailSender;

        public InvitesService(
            IRepository<Invite> invitesRepository,
            IEmailSender emailSender)
        {
            this.invitesRepository = invitesRepository;
            this.emailSender = emailSender;
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

        public async Task<TViewModel> GetByIdAsync<TViewModel>(int? id)
        {
            var invite = await this.invitesRepository.All()
                .Where(i => i.Id == id)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            return invite;
        }

        public async Task<Invite> CreateAsync<TInputModel>(TInputModel model)
        {
            var inputInvite = AutoMapperConfig.MapperInstance.Map<Invite>(model);

            var invite = new Invite
            {
                Email = inputInvite.Email,
                Purpose = inputInvite.Purpose,
                SecurityValue = this.GenerateUniqueSecurityValue(),
                IsInvited = true,
                ExpiredOn = DateTime.UtcNow.AddHours(24),
                RequestExpiredOn = DateTime.UtcNow.AddHours(24),
            };

            await this.invitesRepository.AddAsync(invite);
            await this.invitesRepository.SaveChangesAsync();

            // TODO sendEmail

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
            await this.invitesRepository.SaveChangesAsync();

            return invite;
        }

        public async Task<Invite> ApproveAsync(int id)
        {
            var invite = await this.invitesRepository
                .All()
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();

            invite.IsInvited = true;
            invite.ExpiredOn = DateTime.UtcNow.AddHours(24);
            invite.RequestExpiredOn = DateTime.UtcNow.AddHours(24);

            this.invitesRepository.Update(invite);
            await this.invitesRepository.SaveChangesAsync();

            return invite;
        }

        public async Task<Invite> EditAsync<TInputModel>(TInputModel model)
        {
            var inputInvite = AutoMapperConfig.MapperInstance.Map<Invite>(model);

            var invite = await this.invitesRepository
                .All()
                .Where(i => i.Id == inputInvite.Id)
                .FirstOrDefaultAsync();

            var udatedExiperdOn = inputInvite.ExpiredOn;
            var udatedRequestExiperd = inputInvite.RequestExpiredOn;

            invite.Email = inputInvite.Email;
            invite.ExpiredOn = inputInvite.ExpiredOn;
            invite.RequestExpiredOn = inputInvite.RequestExpiredOn;

            this.invitesRepository.Update(invite);
            await this.invitesRepository.SaveChangesAsync();

            return invite;
        }

        public async Task DeleteAsync(int? id)
        {
            var invite = await this.invitesRepository
                .All()
                .SingleOrDefaultAsync(i => i.Id == id);

            this.invitesRepository.Delete(invite);
            await this.invitesRepository.SaveChangesAsync();

            // var messageToSend = "Your request invitation has been rejected";

            // await this.emailSender.SendEmailAsync(
            //   "admin@admin.bg",
            //   "admin",
            //   this.Input.Email,
            //   "Confirm your email",
            //   $"{messageToSend}");
        }

        private async Task SendEmailAsync(string messageToSend, string email)
        {
            await this.emailSender.SendEmailAsync(
                "admin@admin.bg",
                "admin",
                email,
                "Confirm your email",
                $"{messageToSend}");
        }
    }
}
