// ReSharper disable VirtualMemberCallInConstructor
namespace PlanIt.Data.Models
{
    using System;

    using Microsoft.AspNetCore.Identity;

    using PlanIt.Data.Common.Models;

    public class PlanItRole : IdentityRole, IAuditInfo, IDeletableEntity
    {
        public PlanItRole()
            : this(null)
        {
        }

        public PlanItRole(string name)
            : base(name)
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
