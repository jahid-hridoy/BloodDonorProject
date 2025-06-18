using BloodDonorProject.Data;
using Microsoft.AspNetCore.Mvc;
using BloodDonorProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace BloodDonorProject.Controllers;

public class BloodDonorController : Controller
{
    
    private readonly BloodDonorDbContext _context;
    private readonly IWebHostEnvironment _env;
    private List<SelectListItem> GetBloodGroupSelectList()
    {
        return Enum.GetValues(typeof(BloodGroup))
            .Cast<BloodGroup>()
            .Select(bg => new SelectListItem
            {
                Value = bg.ToString(),
                Text = bg.GetType()
                    .GetMember(bg.ToString())
                    .First()
                    .GetCustomAttribute<DisplayAttribute>()?.Name ?? bg.ToString()
            }).ToList();
    }

    public BloodDonorController(BloodDonorDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Create()
    {
        ViewBag.BloodGroupList = GetBloodGroupSelectList();
        return View();
    }
    
    [HttpPost]
    public IActionResult Create(BloodDonorCreateViewModel donor)
    {
        if (!ModelState.IsValid)
            return View(donor);
        var newDonor = new BloodDonor
        {
            FullName = donor.FullName,
            ContactNumber = donor.ContactNumber,
            DateOfBirth = donor.DateOfBirth,
            Email = donor.Email,
            BloodGroup = donor.BloodGroup,
            Address = donor.Address,
            Weight = donor.Weight,
            LastDonationDate = donor.LastDonationDate
        };
        if (donor.ProfilePicture != null && donor.ProfilePicture.Length > 0)
        {   
            Console.WriteLine(Path.GetExtension(donor.ProfilePicture.FileName));
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(donor.ProfilePicture.FileName)}";
            var filePath = Path.Combine(_env.WebRootPath, "ProfilePictures");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            var fullPath = Path.Combine(filePath, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                donor.ProfilePicture.CopyTo(stream);
                newDonor.ProfilePicture = fullPath;
            }
        }    
        _context.BloodDonors.Add(newDonor);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

}
