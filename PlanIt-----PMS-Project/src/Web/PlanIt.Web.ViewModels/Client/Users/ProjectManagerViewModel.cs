﻿namespace PlanIt.Web.ViewModels.Client.Users
{
    using System.ComponentModel;

    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class ProjectManagerViewModel : IMapFrom<PlanItUser>
    {
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        [DisplayName("Full name")]
        public string FullName => $"{this.FirstName} {this.MiddleName} {this.LastName}";

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}
