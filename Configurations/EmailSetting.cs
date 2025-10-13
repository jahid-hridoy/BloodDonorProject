using System.ComponentModel.DataAnnotations;

namespace BloodDonorProject.Configurations
{
    public class EmailSetting
    {
        [Required]
        public string SmtpServer { get; set; } = string.Empty;
        public int Port { get; set; }
    }
}
