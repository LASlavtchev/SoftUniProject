namespace PlanIt.Web.ViewModels.Administration.Invites
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class InviteSelectViewModel
    {
        public SelectList Invites { get; set; }

        public string InviteId { get; set; }
    }
}
