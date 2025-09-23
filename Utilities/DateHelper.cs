namespace BloodDonorProject.Utilities
{
    public class DateHelper
    {
        public static int CalculateAge(DateTime dateOfBirth)
        {
            return DateTime.Now.Year - dateOfBirth.Year;
        }

        public static string GetLastDonationDate(DateTime? lastDonationDate)
        {
            return lastDonationDate.HasValue
                ? $"{(DateTime.Today - lastDonationDate.Value).Days} days ago"
                : "Never";
        }
    }
}
