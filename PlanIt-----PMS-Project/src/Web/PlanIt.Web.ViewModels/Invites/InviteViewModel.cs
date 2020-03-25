namespace PlanIt.Web.ViewModels.Invites
{
    using System;

    using PlanIt.Data.Models;
    using PlanIt.Services.Mapping;

    public class InviteViewModel : IMapFrom<Invite>
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Purpose { get; set; }

        public string SecurityValue { get; set; }

        public bool IsInvited { get; set; }

        public string HaveInvitation
        {
            get
            {
                string result;

                if (this.IsInvited)
                {
                    result = "Yes";
                }
                else
                {
                    result = "No";
                }

                return result;
            }
        }

        public DateTime? ExpiredOn { get; set; }

        public string InviteExpiredOn
        {
            get
            {
                string result;

                if (this.ExpiredOn == null)
                {
                    result = "-";
                }
                else
                {
                    result = this.ExpiredOn.ToString();
                }

                return result;
            }
        }

        public DateTime RequestExpiredOn { get; set; }
    }
}
