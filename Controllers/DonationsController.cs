using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BloodDonorProject.Data;
using BloodDonorProject.Models;
using BloodDonorProject.Services.Implementations;
using Microsoft.AspNetCore.Razor.TagHelpers;
using BloodDonorProject.Data.Interfaces;
using BloodDonorProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace BloodDonorProject.Controllers
{
    [Authorize(Roles = "Donor,Admin")]
    public class DonationsController : Controller
    {
        private readonly IDonationService _donationService;
        private readonly IBloodDonorService _bloodDonorService;
        private readonly BloodDonorDbContext _context;
        public DonationsController(IDonationService donationService, IBloodDonorService bloodDonorService, BloodDonorDbContext context)
        {
            _donationService = donationService;
            _bloodDonorService = bloodDonorService;
            _context = context;
        }

        // GET: Donations
        public async Task<IActionResult> Index()
        {
            return View(await _donationService.GetAllDonationsAsync());
        }

        // GET: Donations/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var donation = await _donationService.GetDonationByIdAsync(id);
            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }

        // GET: Donations/Create
        public IActionResult Create()
        {
            ViewBag.donorList = new SelectList(_context.BloodDonors, "Id", "FullName");
            return View();
        }

        // POST: Donations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DonationDate,BloodDonorId")] Donation donation)
        {
            if (ModelState.IsValid)
            {
                await _donationService.CreateDonation(donation);
                return RedirectToAction(nameof(Index));
            }
            return View(donation);
        }

        [HttpGet]
        // GET: Donations/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var donation = await _donationService.GetDonationByIdAsync(id);
            if (donation == null)
            {
                return NotFound();
            }
            return View(donation);
        }

        // POST: Donations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DonationDate,BloodDonorId")] Donation donation)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _donationService.UpdateDonation(id, donation);
                }
                catch (Exception)
                {
                    
                }
                return RedirectToAction(nameof(Index));
            }
            return View(donation);
        }

        // GET: Donations/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var donation = await _donationService.GetDonationByIdAsync(id);
            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }

        // POST: Donations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _donationService.DeleteDonation(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
