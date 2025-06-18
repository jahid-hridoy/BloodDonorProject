using Microsoft.AspNetCore.Mvc;
namespace BloodDonorProject.Controllers;

public class BloodDonorController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}
