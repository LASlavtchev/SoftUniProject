﻿namespace PlanIt.Web.ViewModels.Administration.Invites
{
    using System;

    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class InviteExtendInputModel : IMapFrom<Invite>, IMapTo<Invite>
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public bool IsInvited { get; set; }

        public DateTime? ExpiredOn { get; set; }

        public DateTime RequestExpiredOn { get; set; }
    }
}
