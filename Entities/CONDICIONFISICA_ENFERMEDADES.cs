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
    
    public partial class CONDICIONFISICA_ENFERMEDADES
    {
        public CONDICIONFISICA_ENFERMEDADES()
        {
            this.CONDICIONFISICA_INTERNOENFERMEDADES = new HashSet<CONDICIONFISICA_INTERNOENFERMEDADES>();
        }
    
        public int ID { get; set; }
        public string NOMBRECIENTIFICO { get; set; }
        public string NOMBRESINOMIMO { get; set; }
        public string DESCRIPCION { get; set; }
    
        public virtual ICollection<CONDICIONFISICA_INTERNOENFERMEDADES> CONDICIONFISICA_INTERNOENFERMEDADES { get; set; }
    }
}
