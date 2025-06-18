using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BloodDonorProject.Data;
using BloodDonorProject.Models;

namespace BloodDonorProject.Views.BloodDonor
{
    public class IndexModel : PageModel
    {
        private readonly BloodDonorProject.Data.BloodDonorDbContext _context;

        public IndexModel(BloodDonorProject.Data.BloodDonorDbContext context)
        {
            _context = context;
        }

        public IList<Models.BloodDonor> BloodDonor { get;set; } = default!;

        public async Task OnGetAsync()
        {
            BloodDonor = await _context.BloodDonors.ToListAsync();
        }
    }
}