namespace PlanIt.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "PlanIt";

        public const string CompanyOwnerRoleName = "Company owner"; // Super User
        public const string AdministratorRoleName = "Administrator";
        public const string ProjectManagerRoleName = "Project manager";
        public const string ClientRoleName = "Client";
        public const string UserRoleName = "User";
        public const string AdministrationAreaRoleNames = "Company owner, Administrator";
        public const string ManagementAreaRoleNames = "Company owner, User, Project manager";
        public const string ManagementRoleNames = "Company owner, Project manager";

        // EmailAddress validation
        public const string EmailAddressErrorMessage = "Invalid email";

        // User entity names
        public const int MaxLengthUserName = 50;
        public const int MinLengthUserName = 3;
        public const string UserNamesErrorMessage = "The Name must be less than {1} symbols";

        // Password
        public const int MaxLengthPassword = 100;
        public const int MinLengthPassword = 3; // default 6
        public const string ErrorMessagePasswordConfirmation = "The password and confirmation password do not match.";

        // Phone number
        public const string PhoneNumberRegexPattern = @"^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$";
        public const string ErrorMessagePhoneNumber = "Invalid phone nummber.";

        // Other strings
        public const int MaxLengthString = 100;
        public const int MinLengthString = 3; // default 6
        public const string ErrorMessageStringLength = "The {0} must be at least {2} and at max {1} characters long.";

        // DateTime
        public const string ErrorMessageDateTime = "The date and time have to be after now!";

        // Currency
        public const string ErrorMessageNonNegativeNumber = "The number has to be positive";

        // ProgressStatusses
        public const string ProgressStatusNotAssigned = "Not Assigned";
        public const string ProgressStatusAssigned = "Assigned";
        public const string ProgressStatusInProgress = "In progress";
        public const string ProgressStatusCompleted = "Completed";
        public const string ProgressStatusSuspended = "Suspended";
        public const string ProgressStatusCanceled = "Canceled";
        public const string ProgressStatusApproving = "Approving";
    }
}
