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
    
    public partial class MOTIVOSINGRESO
    {
        public int IDINGRESO { get; set; }
        public Nullable<bool> VOLUNTARIAMENTE { get; set; }
        public Nullable<bool> DISPUESTOASUJETARSEPV { get; set; }
        public Nullable<System.DateTime> FECHAFIRMAACUERDO { get; set; }
        public Nullable<System.DateTime> FECHADIAGNOSTICO { get; set; }
        public string DESCRIPCION { get; set; }
    
        public virtual INGRESO INGRESO { get; set; }
    }
}
