using AHPDecision.Models;
using AHPDecision.ViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AHPDecision.ViewModels
{
    public class ProjectViewModel
    {
        public string[] partialViewPodaci = { "Nova alternativa", "Naziv alternative", "Opis alternative" };
        public Projekt Project { get; set; }
        public List<Kriterij> Criteria { get; set; }
        public List<Alternativa> Alternatives { get; set; }
        public List<Dnevnik> Logs { get; set; }
        public ContentHeader ContentHeader { get; set; }

        public ProjectViewModel()
        {
            //this.ContentHeader = GetContentHeader();
        }

        public ProjectViewModel(Projekt project, List<Kriterij> criteria, List<Alternativa> alternatives, List<Dnevnik> logs)
        {
            this.Project = project;
            this.Criteria = criteria;
            this.Alternatives = alternatives;
            this.Logs = logs;
            this.ContentHeader = GetContentHeader(project.naziv);
        }

        private ContentHeader GetContentHeader(string projectName)
        {
            List<Tuple<string, string, string>> path = new List<Tuple<string, string, string>>();
            path.Add(new Tuple<string, string, string>("fa-home", "/Home/Index", "Početna"));
            path.Add(new Tuple<string, string, string>("fa-file-excel-o", null, projectName));


            return new ContentHeader("Kriteriji", projectName, path);
        }

    }
}