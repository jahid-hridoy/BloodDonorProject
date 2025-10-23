namespace BloodDonorProject.Models.ViewModel
{
    public class ManageUserRolesViewModel
    {
        public required string UserId { get; set; }
        public required string Email { get; set; }
        public required List<string> AvailableRoles { get; set; } = [];
        public required List<string> UserRoles { get; set; } = [];
    }
}
