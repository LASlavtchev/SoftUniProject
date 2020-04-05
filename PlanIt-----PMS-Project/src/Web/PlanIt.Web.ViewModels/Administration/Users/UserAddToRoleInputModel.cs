namespace PlanIt.Web.ViewModels.Administration.Users
{
    using System.ComponentModel.DataAnnotations;

    public class UserAddToRoleInputModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
