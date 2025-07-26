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
    public IActionResult Index(string bloodGroup, string address, bool? eligible)
    {
        IQueryable<BloodDonor> query = _context.BloodDonors;
        if (!string.IsNullOrEmpty(bloodGroup))
        {
            query = query.Where(d => d.BloodGroup.ToString() == bloodGroup);
        }

        if (!string.IsNullOrEmpty(address))
        {
            query = query.Where(d => d.Address != null && d.Address.ToString() == address);
        }
        var donors = query.Select(d => new BloodDonorListViewModel
        {
            Id = d.Id,
            FullName = d.FullName,
            ContactNumber = d.ContactNumber,
            Age = DateTime.Now.Year - d.DateOfBirth.Year,
            Email = d.Email,
            BloodGroup = d.BloodGroup.ToString(),
            Address = d.Address,
            ProfilePicture = d.ProfilePicture,
            LastDonationDate = d.LastDonationDate.HasValue? $"{(DateTime.Today - d.LastDonationDate.Value).Days} days ago": "Never",
            IsEligibleForDonation = (d.Weight >= 50 && d.Weight <= 150) && (d.LastDonationDate == null || (DateTime.Now - d.LastDonationDate.Value).TotalDays >= 90)
        }).ToList();
        // var donors = query.ToList();
        if (eligible.HasValue)
        {
            donors = donors.Where(d => d.IsEligibleForDonation == eligible).ToList();
        }
        return View(donors);
    }

    public IActionResult Create()
    {
        ViewBag.BloodGroupList = GetBloodGroupSelectList();
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync(BloodDonorCreateViewModel donor)
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
                await donor.ProfilePicture.CopyToAsync(stream);
            }
            newDonor.ProfilePicture = Path.Combine("ProfilePictures", fileName);
        }    
        _context.BloodDonors.Add(newDonor);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Details(int id)
    {
        var donor = _context.BloodDonors.FirstOrDefault(d => d.Id == id);
        if (donor == null)
        {
            return NotFound();
        }
        var viewModel = new BloodDonorListViewModel
        {
            Id = donor.Id,
            FullName = donor.FullName,
            ContactNumber = donor.ContactNumber,
            Age = DateTime.Now.Year - donor.DateOfBirth.Year,
            Email = donor.Email,
            BloodGroup = donor.BloodGroup.ToString(),
            Address = donor.Address,
            ProfilePicture = donor.ProfilePicture,
            LastDonationDate = donor.LastDonationDate.HasValue ? $"{(DateTime.Today - donor.LastDonationDate.Value).Days} days ago" : "Never",
            IsEligibleForDonation = (donor.Weight >= 50 && donor.Weight <= 150) && (donor.LastDonationDate == null || (DateTime.Now - donor.LastDonationDate.Value).TotalDays >= 90)
        };
        return View(viewModel);
    }
    
    public IActionResult Edit(int id)
    {
        var donor = _context.BloodDonors.FirstOrDefault(d => d.Id == id);
        if (donor == null)
        {
            return NotFound();
        }
        var viewModel = new BloodDonorEditViewModel
        {
            Id = donor.Id,
            FullName = donor.FullName,
            ContactNumber = donor.ContactNumber,
            DateOfBirth = donor.DateOfBirth,
            Email = donor.Email,
            BloodGroup = donor.BloodGroup,
            Address = donor.Address,
            Weight = donor.Weight,
            LastDonationDate = donor.LastDonationDate,
            ExistingProfilePicture = donor.ProfilePicture
        };
        return View(viewModel);
    }
    
    [HttpPost]
    [HttpPut]
    public async Task<IActionResult> EditAsync(BloodDonorEditViewModel donor)
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
                await donor.ProfilePicture.CopyToAsync(stream);
            }
            newDonor.ProfilePicture = Path.Combine("ProfilePictures", fileName);
        }    
        _context.BloodDonors.Update(newDonor);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
    
    public IActionResult Delete(int id)
    {
        var donor = _context.BloodDonors.FirstOrDefault(d => d.Id == id);
        if (donor == null)
        {
            return NotFound();
        }
        var viewModel = new BloodDonorListViewModel
        {
            Id = donor.Id,
            FullName = donor.FullName,
            ContactNumber = donor.ContactNumber,
            Age = DateTime.Now.Year - donor.DateOfBirth.Year,
            Email = donor.Email,
            BloodGroup = donor.BloodGroup.ToString(),
            Address = donor.Address,
            ProfilePicture = donor.ProfilePicture,
            LastDonationDate = donor.LastDonationDate.HasValue ? $"{(DateTime.Today - donor.LastDonationDate.Value).Days} days ago" : "Never",
            IsEligibleForDonation = (donor.Weight >= 50 && donor.Weight <= 150) && (donor.LastDonationDate == null || (DateTime.Now - donor.LastDonationDate.Value).TotalDays >= 90)
        };
        return View(viewModel);
    }
    
    [ActionName("DeleteConfirmed")]
    public IActionResult DeleteConfirmed(int id)
    {
        var donor = _context.BloodDonors.FirstOrDefault(d => d.Id == id);
        if (donor == null)
        {
            return NotFound();
        }
        _context.BloodDonors.Remove(donor);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

}

