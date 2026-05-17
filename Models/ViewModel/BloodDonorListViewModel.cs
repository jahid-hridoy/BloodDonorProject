using System.ComponentModel.DataAnnotations;
namespace BloodDonorProject.Models;

public class BloodDonorListViewModel
{
    public int Id { get; set; }
    [Required]
    public string FullName { get; set; }
    [Required]
    public string ContactNumber { get; set; }
    [Required]
    public int Age { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string BloodGroup { get; set; }
    public string Address { get; set; }
    public string? ProfilePicture { get; set; }
    [Required]
    public string LastDonationDate { get; set; }
    public bool IsEligibleForDonation { get; set; }
    public List<DonationHistoryViewModel> DonationHistory { get; set; } = new();
    }

    public class DonationHistoryViewModel
    {
    public int Id { get; set; }
    public DateTime DonationDate { get; set; }
    }