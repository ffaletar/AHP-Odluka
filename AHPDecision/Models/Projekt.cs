//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AHPDecision.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Projekt
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Projekt()
        {
            this.Alternativas = new HashSet<Alternativa>();
            this.Dnevniks = new HashSet<Dnevnik>();
            this.Kriterijs = new HashSet<Kriterij>();
        }
    
        public int id { get; set; }
        public string naziv { get; set; }
        public string opis { get; set; }
        public System.DateTime datum { get; set; }
        public Nullable<System.DateTime> zadnjaPromjena { get; set; }
        public int fazaProjekta { get; set; }
        public string korisnik { get; set; }
        public Nullable<bool> konzistentno { get; set; }
        public Nullable<bool> obrisan { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Alternativa> Alternativas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Dnevnik> Dnevniks { get; set; }
        public virtual FazaProjekta FazaProjekta1 { get; set; }
        public virtual Korisnik Korisnik1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Kriterij> Kriterijs { get; set; }
    }
}
