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
    
    public partial class CONT_TALONARIO
    {
        public CONT_TALONARIO()
        {
            this.CONT_CODOTALONARIO = new HashSet<CONT_CODOTALONARIO>();
        }
    
        public int ID { get; set; }
        public Nullable<System.DateTime> FECHACREADO { get; set; }
        public string ESTADO { get; set; }
        public string DESCRIPCION { get; set; }
        public Nullable<System.DateTime> FECHACANCELADO { get; set; }
        public Nullable<int> IDINGRESO { get; set; }
    
        public virtual ICollection<CONT_CODOTALONARIO> CONT_CODOTALONARIO { get; set; }
        public virtual INGRESO INGRESO { get; set; }
    }
}
