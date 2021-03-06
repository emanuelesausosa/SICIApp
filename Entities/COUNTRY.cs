//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SICIApp.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class COUNTRY
    {
        public COUNTRY()
        {
            this.CITies = new HashSet<CITY>();
            this.COUNTRYLANGUAGEs = new HashSet<COUNTRYLANGUAGE>();
            this.DATOSPERSONALES1 = new HashSet<DATOSPERSONALES1>();
        }
    
        public string CODE { get; set; }
        public string NAME { get; set; }
        public string CONTINENT { get; set; }
        public string REGION { get; set; }
        public decimal SURFACEAREA { get; set; }
        public Nullable<short> INDEPYEAR { get; set; }
        public int POPULATION { get; set; }
        public Nullable<decimal> LIFEEXPECTANCY { get; set; }
        public Nullable<decimal> GNP { get; set; }
        public Nullable<decimal> GNPOLD { get; set; }
        public string LOCALNAME { get; set; }
        public string GOVERNMENTFORM { get; set; }
        public string HEADOFSTATE { get; set; }
        public Nullable<int> CAPITAL { get; set; }
        public string CODE2 { get; set; }
    
        public virtual ICollection<CITY> CITies { get; set; }
        public virtual ICollection<COUNTRYLANGUAGE> COUNTRYLANGUAGEs { get; set; }
        public virtual ICollection<DATOSPERSONALES1> DATOSPERSONALES1 { get; set; }
    }
}
