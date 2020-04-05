namespace PlanIt.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using PlanIt.Data.Common.Repositories;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<PlanItUser> userRepository;

        public UsersService(IDeletableEntityRepository<PlanItUser> userRepository)
        {
            this.userRepository = userRepository;
        }

        public int GetAllWithDeletedCount()
        {
            var allWithDeletedCount = this.userRepository
                .AllWithDeleted()
                .Count();

            return allWithDeletedCount;
        }

        public int GetAllCount()
        {
            var allWithDeletedCount = this.userRepository
                .All()
                .Count();

            return allWithDeletedCount;
        }

        public int GetAllDeletedCount()
        {
            var allDeletedCount = this.userRepository
                .AllWithDeleted()
                .Where(u => u.IsDeleted == true)
                .Count();

            return allDeletedCount;
        }

        public async Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>(ClaimsPrincipal user)
        {
            var currentUser = await this.userRepository
                .All()
                .FirstOrDefaultAsync(u => u.Email == user.Identity.Name);

            var users = await this.userRepository
                .All()
                .Where(u => u.IsDeleted == false && u.Id != currentUser.Id)
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

        public async Task<TViewModel> GetDeletedByIdAsync<TViewModel>(string id)
        {
            var user = await this.userRepository
                .AllWithDeleted()
                .Where(u => u.Id == id && u.IsDeleted)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task DeleteAsync(string id)
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
    }
}
