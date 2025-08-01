using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
namespace BloodDonorProject.Models;

public class BloodDonor
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string FullName { get; set; }
    [Required]
    [Phone]
    public string ContactNumber { get; set; }
    [Required]
    public DateTime DateOfBirth { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public BloodGroup BloodGroup { get; set; }
    [Range(50,150)]
    [Display(Name = "Weight (kg)")]
    public float Weight { get; set; }
    public string Address { get; set; }
    public string? ProfilePicture { get; set; }
    public DateTime? LastDonationDate { get; set; }
    public Collection<Donation> Donations { get; set; } =  new Collection<Donation>();
}