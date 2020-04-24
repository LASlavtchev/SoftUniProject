namespace PlanIt.Services.CronJobs
{
    using System.Threading.Tasks;

    public interface IInviteDbCleanupJob
    {
        Task Work();
    }
}
