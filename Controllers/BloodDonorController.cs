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

    public BloodDonorController(BloodDonorDbContext context)
    {
        _context = context;
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
    public IActionResult Create(BloodDonor donor)
    {
        if (ModelState.IsValid)
        {
            _context.BloodDonors.Add(donor);
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }

}
