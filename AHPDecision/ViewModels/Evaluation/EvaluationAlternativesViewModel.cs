using AHPDecision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AHPDecision.ViewModels
{
    public class EvaluationAlternativesViewModel
    {
        public Projekt Projekt { get; set; }
        public Kriterij Kriterij { get; set; }

        public List<AlternativaKriterij> AlternativaKriterij { get; set; }

        public List<UsporedbaAlternativaPremaKriteriju> UsporedbaAlternativaPremaKriteriju { get; set; }

        public List<Kriterij> Kriteriji { get; set; }
        public List<Alternativa> Alternative { get; set; }
        public List<AlternativesComparedViewModel> ListaKombinacijaAlternativa { get; set; }





    }
}