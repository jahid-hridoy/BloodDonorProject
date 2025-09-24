using BloodDonorProject.Services.Interfaces;

namespace BloodDonorProject.Services.Implementations;

public class FileService: IFileService
{
    private readonly IWebHostEnvironment _env;

    public FileService(IWebHostEnvironment env)
    {
        _env = env;
    }
    public async Task<string> SaveFileAsync(IFormFile file)
    {
        if (file != null && file.Length > 0)
        {   
            Console.WriteLine(Path.GetExtension(file.FileName));
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(_env.WebRootPath, "ProfilePictures");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            var fullPath = Path.Combine(filePath, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return Path.Combine("ProfilePictures", fileName);
        }
        return null!;
    }
}