using AHPDecision.Models;
using AHPDecision.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace AHPDecision.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            AHPEntities4 db = new AHPEntities4();
            var userId = User.Identity.GetUserId();
            IEnumerable<Projekt> projekti = db.Projekts.Where(c => (c.korisnik == userId && c.obrisan != true)).OrderByDescending(c => c.zadnjaPromjena).ToList();

            HomeViewModel homeViewModel = new HomeViewModel(projekti);

            return View(homeViewModel);
        }
    }
}