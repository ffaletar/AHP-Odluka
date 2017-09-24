using AHPDecision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AHPDecision.ViewModels
{
    public class CriteriaTreeViewModel
    {
        public List<Kriterij> kriteriji { get; set; }
        public Projekt projekt { get; set; }
        public Kriterij roditelj { get; set; }

    }
}