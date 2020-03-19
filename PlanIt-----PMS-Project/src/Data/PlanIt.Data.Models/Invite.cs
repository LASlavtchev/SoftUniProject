namespace PlanIt.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using PlanIt.Common;
    using PlanIt.Data.Common.Models;

    public class Invite : BaseModel<int>
    {
        [Required]
        [EmailAddress(ErrorMessage = GlobalConstants.EmailAddressErrorMessage)]
        public string Email { get; set; }

        public string Purpose { get; set; }

        public string SecurityValue { get; set; }

        public bool IsInvited { get; set; }

        public DateTime? ExpiredOn { get; set; }

        public DateTime RequestExpiredOn { get; set; }
    }
}
