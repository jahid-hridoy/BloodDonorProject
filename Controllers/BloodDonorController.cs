using BloodDonorProject.Data;
using Microsoft.AspNetCore.Mvc;
using BloodDonorProject.Models;

namespace BloodDonorProject.Controllers;

public class BloodDonorController : Controller
{
    
    private readonly BloodDonorDbContext _context;

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
