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
    
    public partial class PRO_CRITERIO
    {
        public PRO_CRITERIO()
        {
            this.PRO_NIVELCRITERIO = new HashSet<PRO_NIVELCRITERIO>();
        }
    
        public int ID { get; set; }
        public string NOMBRE { get; set; }
        public string DESCRIPCION { get; set; }
    
        public virtual ICollection<PRO_NIVELCRITERIO> PRO_NIVELCRITERIO { get; set; }
    }
}
