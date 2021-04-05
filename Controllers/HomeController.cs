using BowlingLeague.Models;
using BowlingLeague.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BowlingLeague.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private BowlingLeagueContext context { get; set; }

        public HomeController(ILogger<HomeController> logger, BowlingLeagueContext ctx)
        {
            _logger = logger;
            context = ctx;
        }

        public IActionResult Index(int? teamId, string teamName, int pageNum = 0)
        {

            int pageSize = 5;
            // Parameterized Linq. Probs better for security, avoid possible SQL injection
            // return View(context.Bowlers.Where(x => x.BowlerLastName.Contains("V")).OrderBy(x => x.BowlerLastName).ToList()); 

            // Raw SQL
            // return View(context.Bowlers.FromSqlRaw("SELECT * FROM Bowlers WHERE BowlerLastName LIKE \"V%\"").ToList());

            // Interpolated
            // return View(context.Bowlers.FromSqlInterpolated($"SELECT * FROM Bowlers WHERE TeamId = {teamId} OR {teamId} IS NULL"));

            // Linq
            return View(new IndexViewModel
            {
                Bowlers = context.Bowlers.Where(t => t.TeamId == teamId || teamId == null).OrderBy(b => b.BowlerLastName).Skip((pageNum - 1) * pageSize).Take(pageSize).ToList(),
                PageNumberingInfo = new PageNumberingInfo
                {
                    NumItemsPerPage = pageSize,
                    CurrentPage = pageNum,
                    // If specific team is not specified, return count of all bowlers. If team is specified, return count of bowlers on that team
                    TotalNumItems = (teamId == null ? context.Bowlers.Count() : context.Bowlers.Where(t => t.TeamId == teamId).Count())
                },
                TeamName = teamName
            });

            // Can also use .FromSqlInterpolated($"SELECT * FROM {Table} WHERE {Condition}") etc for parameterized SQL
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
}
