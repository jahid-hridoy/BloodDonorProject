using Microsoft.AspNetCore.Mvc;
using BloodDonorProject.Models;

namespace BloodDonorProject.Controllers;

public class BloodDonorController : Controller
{
    // GET
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
        return RedirectToAction("Index");
    }

}
