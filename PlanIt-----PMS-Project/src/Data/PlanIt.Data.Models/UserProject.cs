namespace PlanIt.Data.Models
{
    using System;

    using PlanIt.Data.Common.Models;

    public class UserProject : IAuditInfo, IDeletableEntity
    {
        public string PlanItUserId { get; set; }

        public PlanItUser User { get; set; }

        public int ProjectId { get; set; }

        public Project Project { get; set; }

        // AuditInfo
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
