using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AHPDecision.Models
{
    public class AlternativesComparedViewModel
    {
        public Alternativa Alternativa1 { get; set; }
        public Alternativa Alternativa2 { get; set; }

        public AlternativesComparedViewModel()
        {

        }

        public AlternativesComparedViewModel(Alternativa alternativa1, Alternativa alternativa2)
        {
            this.Alternativa1 = alternativa1;
            this.Alternativa2 = alternativa2;
        }
    }
}
