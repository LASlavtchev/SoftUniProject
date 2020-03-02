// ReSharper disable VirtualMemberCallInConstructor
namespace PlanIt.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Identity;
    using PlanIt.Common;
    using PlanIt.Data.Common.Models;

    public class PlanItUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public PlanItUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.UserProjects = new HashSet<UserProject>();
        }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        // Additional properties
        [Required]
        [MaxLength(GlobalConstants.MaxLengthUserName, ErrorMessage = GlobalConstants.UserNamesErrorMessage)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(GlobalConstants.MaxLengthUserName, ErrorMessage = GlobalConstants.UserNamesErrorMessage)]
        public string MiddleName { get; set; }

        [Required]
        [MaxLength(GlobalConstants.MaxLengthUserName, ErrorMessage = GlobalConstants.UserNamesErrorMessage)]
        public string LastName { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string Occupation { get; set; }

        public virtual ICollection<UserProject> UserProjects { get; set; }
    }
}
