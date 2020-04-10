namespace PlanIt.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using PlanIt.Data.Common.Repositories;
    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class ClientsService : IClientsServices
    {
        private readonly IDeletableEntityRepository<Client> clientRepository;

        public ClientsService(IDeletableEntityRepository<Client> clientRepository)
        {
            this.clientRepository = clientRepository;
        }

        public async Task<Client> CreateAsync(string userId)
        {
            var client = new Client
            {
                PlantItUserId = userId,
            };

            await this.clientRepository.AddAsync(client);
            await this.clientRepository.SaveChangesAsync();

            return client;
        }

        public async Task DeleteAsync(string userId)
        {
            var client = await this.clientRepository
                .All()
                .FirstOrDefaultAsync(c => c.PlantItUserId == userId);

            this.clientRepository.Delete(client);
            await this.clientRepository.SaveChangesAsync();
        }

        public async Task RestoreAsync(string userId)
        {
            var client = await this.clientRepository
                .AllWithDeleted()
                .FirstOrDefaultAsync(c => c.PlantItUserId == userId);

            this.clientRepository.Undelete(client);
            await this.clientRepository.SaveChangesAsync();
        }

        public bool IsDeletedClientExist(string userId)
        {
            var client = this.clientRepository
                .AllWithDeleted()
                .FirstOrDefault(c => c.PlantItUserId == userId);

            if (client == null)
            {
                return false;
            }

            return true;
        }

        public async Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>()
        {
            var all = await this.clientRepository
                .All()
                .To<TViewModel>()
                .ToListAsync();

            return all;
        }

        public int GetAllCount()
        {
            var allCount = this.clientRepository
                .All()
                .Count();

            return allCount;
        }

        public async Task<TViewModel> GetClientByUserIdAsync<TViewModel>(string userId)
        {
            var client = await this.clientRepository
                .All()
                .Where(c => c.PlantItUserId == userId)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            return client;
        }

        public async Task<int> GetClientIdByUserIdAsync(string userId)
        {
            var client = await this.clientRepository
                .All()
                .Where(c => c.PlantItUserId == userId)
                .FirstOrDefaultAsync();

            return client.Id;
        }
    }
}
