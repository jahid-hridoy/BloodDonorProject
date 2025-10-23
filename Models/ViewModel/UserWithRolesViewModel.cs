namespace BloodDonorProject.Models.ViewModel
{
    public class UserWithRolesVIewModel
    {
        public required string UserId { get; set; }
        public required string Email { get; set; }
        public required List<string> Roles { get; set; } = [];
    }
}
