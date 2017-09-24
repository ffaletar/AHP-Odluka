using AHPDecision.Models;
using AHPDecision.ViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AHPDecision.ViewModels
{
    public class HomeViewModel
    {
        public string[] partialViewPodaci = { "Novi projekt", "Naziv projekta", "Opis projekta" };
        public IEnumerable<Projekt> Projects { get; set; }
        public ContentHeader ContentHeader { get; private set; }

        public HomeViewModel()
        {
            ContentHeader = GetContentHeader();
        }

        public HomeViewModel(IEnumerable<Projekt> projects)
        {
            ContentHeader = GetContentHeader();

            this.Projects = projects;
        }

        private ContentHeader GetContentHeader()
        {
            List<Tuple<string, string, string>> path = new List<Tuple<string, string, string>>();
            path.Add(new Tuple<string, string, string>("fa-home", "/Home/Index", "Početna"));
            path.Add(new Tuple<string, string, string>("fa-tasks", null, "Projekti"));

            return new ContentHeader("Početna", "Projekti", path);
        }
    }
}