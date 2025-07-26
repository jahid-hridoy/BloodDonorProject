using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloodDonorProject.Models;

public class Donation
{   
    [Key]
    public int Id { get; set; }
    [Required]
    public DateTime DonationDate { get; set; }
    [Required] 
    [ForeignKey("BloodDonor")]
    public int BloodDonorId { get; set; }
}