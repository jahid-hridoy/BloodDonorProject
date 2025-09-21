using BloodDonorProject.Data;
using Microsoft.AspNetCore.Mvc;
using BloodDonorProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using BloodDonorProject.Services.Interfaces;

namespace BloodDonorProject.Controllers;

public class BloodDonorController : Controller
{
    
    private readonly BloodDonorDbContext _context;
    private readonly IFileService _fileService;
    private readonly IBloodDonorService _bloodDonorService;
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

    public BloodDonorController(BloodDonorDbContext context, IFileService fileService, IBloodDonorService bloodDonorService)
    {
        _context = context;
        _fileService = fileService;
        _bloodDonorService = bloodDonorService;
    }
    public async Task<IActionResult> Index(string bloodGroup, string address, bool? eligible)
    {
        var donors = await _bloodDonorService.GetAllAsync(bloodGroup, address, eligible);
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
            LastDonationDate = donor.LastDonationDate,
            ProfilePicture = await _fileService.SaveFileAsync(donor.ProfilePicture)
        };
        _context.BloodDonors.Add(newDonor);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Details(int id)
    {
        var donor = await _bloodDonorService.GetByIdAsync(id);
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
    public async Task<IActionResult> EditAsync(BloodDonorEditViewModel donor)
    {
        if (!ModelState.IsValid)
            return View(donor);
        var newDonor = new BloodDonor
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
        };
        newDonor.ProfilePicture = await _fileService.SaveFileAsync(donor.ProfilePicture) ?? donor.ExistingProfilePicture;
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

