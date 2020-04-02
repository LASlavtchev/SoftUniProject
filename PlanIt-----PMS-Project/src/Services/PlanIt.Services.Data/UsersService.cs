namespace PlanIt.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using PlanIt.Data.Common.Repositories;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class UsersService : IUsersService
    {
        private readonly UserManager<PlanItUser> userManager;
        private readonly IDeletableEntityRepository<PlanItUser> userRepository;

        public UsersService(
            UserManager<PlanItUser> userManager,
            IDeletableEntityRepository<PlanItUser> userRepository)
        {
            this.userManager = userManager;
            this.userRepository = userRepository;
        }

        public async Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>(ClaimsPrincipal user)
        {
            var currentUser = await this.userManager
                .GetUserAsync(user);

            var users = await this.userRepository
                .All()
                .Where(u => u.Id != currentUser.Id)
                .To<TViewModel>()
                .ToListAsync();

            return users;
        }

        public async Task<IEnumerable<TViewModel>> GetAllDeletedAsync<TViewModel>()
        {
            var users = await this.userRepository
                .AllWithDeleted()
                .Where(u => u.IsDeleted == true)
                .To<TViewModel>()
                .ToListAsync();

            return users;
        }

        public async Task<TViewModel> GetByIdAsync<TViewModel>(string id)
        {
            var user = await this.userRepository
                .All()
                .Where(u => u.Id == id)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<TViewModel> GetDeletedByIdAsync<TViewModel>(string id)
        {
            var user = await this.userRepository
                .AllWithDeleted()
                .Where(u => u.Id == id && u.IsDeleted)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task SoftDeleteAsync(string id)
        {
            var user = await this.userRepository
                .All()
                .SingleOrDefaultAsync(i => i.Id == id);

            this.userRepository.Delete(user);
            await this.userRepository.SaveChangesAsync();
        }

        public async Task RestoreAsync(string id)
        {
            var user = await this.userRepository
                .AllWithDeleted()
                .SingleOrDefaultAsync(i => i.Id == id);

            this.userRepository.Undelete(user);
            await this.userRepository.SaveChangesAsync();
        }

        public async Task HardDeleteAsync(string id)
        {
            var user = await this.userRepository
                .AllWithDeleted()
                .SingleOrDefaultAsync(i => i.Id == id && i.IsDeleted);

            var claims = await this.userManager.GetClaimsAsync(user);

            // await this.userManager.RemoveFromRolesAsync();
            await this.userManager.RemoveClaimsAsync(user, claims);
            await this.userManager.DeleteAsync(user);
        }
    }
}
