using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mediver_server.Models;
using mediver_server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace mediver_server.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> SetRank(Uri uri, float rankValue, [FromServices] ApplicationDbContext db)
        {
            var rankedSite = await db.RankedSites.FirstOrDefaultAsync(x => x.Uri == uri);
            if (rankedSite == null)
            {
                rankedSite = new RankedSite
                {
                    Uri = uri,
                    Ignore = false,
                };
                db.Add(rankedSite);
            }

            await db.SaveChangesAsync();

            var rank = await db.Rankings.FirstOrDefaultAsync(x => x.RankedSiteId == uri);
            if (rank == null)
            {
                rank = new RankingEntry
                {
                    RankedSiteId = uri,
                    RankingUserId = this.User.Claims.First(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value,
                };
                db.Add(rank);
            }

            rank.Rank = rankValue;

            await db.SaveChangesAsync();

            return Ok();
        }

        [AllowAnonymous]
        public async Task<IActionResult> GetRank(Uri uri, [FromServices] ApplicationDbContext db)
        {
            var rank = await db.RankedSites
                .Include(x => x.Rankings)
                    .ThenInclude(x => x.RankedSite)
                .Include(x => x.Rankings)
                    .ThenInclude(x => x.RankingUser)
                .FirstOrDefaultAsync(x => x.Uri == uri);

            return Json(new
            {
                rank = rank?.Score,
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
