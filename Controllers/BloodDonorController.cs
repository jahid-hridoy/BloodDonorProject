using BloodDonorProject.Data;
using Microsoft.AspNetCore.Mvc;
using BloodDonorProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using BloodDonorProject.Services.Interfaces;
using BloodDonorProject.Mapping;
using AutoMapper;

namespace BloodDonorProject.Controllers;

public class BloodDonorController : Controller
{
    
    private readonly BloodDonorDbContext _context;
    private readonly IFileService _fileService;
    private readonly IBloodDonorService _bloodDonorService;
    private readonly IMapper _mapper;
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

    public BloodDonorController(BloodDonorDbContext context, IFileService fileService, IBloodDonorService bloodDonorService, IMapper mapper)
    {
        _context = context;
        _fileService = fileService;
        _bloodDonorService = bloodDonorService;
        _mapper = mapper;
    }
    public async Task<IActionResult> Index(string bloodGroup, string address, bool? eligible)
    {
        var donors = await _bloodDonorService.GetAllAsync(bloodGroup, address, eligible);
        var viewModelList = _mapper.Map<List<BloodDonorListViewModel>>(donors);
        return View(viewModelList);
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
        var newDonor = _mapper.Map<BloodDonor>(donor);
        newDonor.ProfilePicture = await _fileService.SaveFileAsync(donor.ProfilePicture);
        await _context.BloodDonors.AddAsync(newDonor);
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
        var viewModel = _mapper.Map<BloodDonorListViewModel>(donor);
        return View(viewModel);
    }
    
    public IActionResult Edit(int id)
    {
        var donor = _context.BloodDonors.FirstOrDefault(d => d.Id == id);
        if (donor == null)
        {
            return NotFound();
        }
        var viewModel = _mapper.Map<BloodDonorEditViewModel>(donor);
        return View(viewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> EditAsync(BloodDonorEditViewModel donor)
    {
        if (!ModelState.IsValid)
            return View(donor);
        var newDonor = _mapper.Map<BloodDonor>(donor);
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
        var viewModel = _mapper.Map<BloodDonorListViewModel>(donor);
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

