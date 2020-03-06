namespace PlanIt.Data.Models
{
    using PlanIt.Data.Common.Models;

    public class Client : BaseDeletableModel<int>
    {
        public string PlantItUserId { get; set; }

        public PlanItUser PlantItUser { get; set; }
    }
}
