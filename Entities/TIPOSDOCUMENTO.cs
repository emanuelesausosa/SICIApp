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
    
    public partial class TIPOSDOCUMENTO
    {
        public TIPOSDOCUMENTO()
        {
            this.DOCUMENTOSINGRESO = new HashSet<DOCUMENTOSINGRESO>();
        }
    
        public int ID { get; set; }
        public string NOMBRETIPO { get; set; }
        public string DESCRIPCION { get; set; }
    
        public virtual ICollection<DOCUMENTOSINGRESO> DOCUMENTOSINGRESO { get; set; }
    }
}
