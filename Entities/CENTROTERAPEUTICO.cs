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
    
    public partial class CENTROTERAPEUTICO
    {
        public CENTROTERAPEUTICO()
        {
            this.USUARIOCENTROCONSULTAs = new HashSet<USUARIOCENTROCONSULTA>();
        }
    
        public string ID { get; set; }
        public string NOMBRE { get; set; }
        public string DESCRIPCION { get; set; }
        public Nullable<int> IDCIUDADOPERACION { get; set; }
    
        public virtual CITY CITY { get; set; }
        public virtual ICollection<USUARIOCENTROCONSULTA> USUARIOCENTROCONSULTAs { get; set; }
    }
}