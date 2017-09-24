using AHPDecision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AHPDecision.ViewModels
{
    public class CriteriaParentsGroupedByLevelsViewModel
    {
        public int Level { get; private set; }
        public Kriterij Roditelj { get; private set; }

        public CriteriaParentsGroupedByLevelsViewModel(int level, Kriterij roditelj)
        {
            this.Level = level;
            this.Roditelj = roditelj;
        }
    }
}