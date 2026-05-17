using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BloodDonorProject.Models;
using BloodDonorProject.Models.ViewModel;
using BloodDonorProject.Services.Interfaces;

namespace BloodDonorProject.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IBloodDonorService _bloodDonorService;
    private readonly IDonationService _donationService;

    public HomeController(ILogger<HomeController> logger, IBloodDonorService bloodDonorService, IDonationService donationService)
    {
        _logger = logger;
        _bloodDonorService = bloodDonorService;
        _donationService = donationService;
    }

    public async Task<IActionResult> Index()
    {
        var donors = _bloodDonorService.GetAllAsync(null, null, null);
        var donations = await _donationService.GetAllDonationsAsync();

        var viewModel = new HomeViewModel
        {
            TotalDonors = donors.Count(),
            TotalDonations = donations.Count()
        };

        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}