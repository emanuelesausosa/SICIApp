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
    
    public partial class CITY
    {
        public CITY()
        {
            this.DATOSPERSONALES = new HashSet<DATOSPERSONALE>();
        }
    
        public int ID { get; set; }
        public string NAME { get; set; }
        public string COUNTRYCODE { get; set; }
        public string DISTRICT { get; set; }
        public int POPULATION { get; set; }
    
        public virtual COUNTRY COUNTRY { get; set; }
        public virtual ICollection<DATOSPERSONALE> DATOSPERSONALES { get; set; }
    }
}