using AHPDecision.Models;
using AHPDecision.ViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AHPDecision.ViewModels
{
    public class EvaluationCriteriaViewModel
    {
        public Projekt Projekt { get; set; }
        public Kriterij KriterijRoditelj { get; set; }

        public List<UsporedbaKriterija> ListaUsporedaba { get; set; }

        public List<Kriterij> ListaKriterija { get; set; }
        public ContentHeader ContentHeader { get; set; }

        public EvaluationCriteriaViewModel()
        {

        }

        public EvaluationCriteriaViewModel(Projekt project, Kriterij criteria, List<UsporedbaKriterija> criteriaComparison, List<Kriterij> listOfCriteria)
        {
            this.Projekt = project;
            this.KriterijRoditelj = criteria;
            this.ListaUsporedaba = criteriaComparison;
            this.ListaKriterija = listOfCriteria;
            this.ContentHeader = GetContentHeader(project.naziv, criteria.naziv);
        }

        private ContentHeader GetContentHeader(string projectName, string criteria)
        {
            List<Tuple<string, string, string>> path = new List<Tuple<string, string, string>>();
            path.Add(new Tuple<string, string, string>("fa-home", "/Home/Index", "Početna"));
            path.Add(new Tuple<string, string, string>("fa-caret-square-o-right", null, "Evaluacija"));
            if(projectName != null)
            {
                path.Add(new Tuple<string, string, string>("fa-file-excel-o", null, projectName));
            }
            if(criteria != null)
            {
                path.Add(new Tuple<string, string, string>("fa-tag", null, criteria));

            }
            
            return new ContentHeader(criteria, projectName, path);
        }
    }
}