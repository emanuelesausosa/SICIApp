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
    
    public partial class APARATOSSISTEMAS_SISTEMAS
    {
        public APARATOSSISTEMAS_SISTEMAS()
        {
            this.APARATOSSISTEMAS = new HashSet<APARATOSSISTEMAS>();
        }
    
        public int ID { get; set; }
        public string NOMBRE { get; set; }
        public string DESCRIPCION { get; set; }
    
        public virtual ICollection<APARATOSSISTEMAS> APARATOSSISTEMAS { get; set; }
    }
}
