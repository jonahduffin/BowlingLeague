using BowlingLeague.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BowlingLeague.Components
{
    public class BowlerTeamViewComponent : ViewComponent
    {
        public BowlingLeagueContext context { get; set; }
        public BowlerTeamViewComponent(BowlingLeagueContext ctx)
        {
            context = ctx;
        }
        public IViewComponentResult Invoke()
        {
            // pulls team name from route/url. Might need to change that.
            ViewBag.SelectedTeam = RouteData?.Values["teamname"];
            return View(context.Teams.Distinct().OrderBy(x => x.TeamName).ToList());
        }
    }
}
