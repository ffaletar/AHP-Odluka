using AHPDecision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AHPDecision.ViewModels
{
    public class CalculatedCriteriaValuesViewModel
    {
        public Kriterij Kriterij { get; set; }
        public double? vrijednost { get; set; }
    }
}