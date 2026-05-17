namespace BloodDonorProject.Models.ViewModel;

public class HomeViewModel
{
    public int TotalDonors { get; set; }
    public int TotalDonations { get; set; }
    public int LivesSaved => TotalDonations * 3; 
}
