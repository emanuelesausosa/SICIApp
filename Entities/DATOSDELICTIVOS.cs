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
    
    public partial class DATOSDELICTIVOS
    {
        public int IDINGRESO { get; set; }
        public Nullable<bool> HACOMETIDO { get; set; }
        public string DETALLESACTOS { get; set; }
        public Nullable<bool> HAESTADOPRESO { get; set; }
        public string DESTALLESRECLUSION { get; set; }
        public Nullable<System.DateTime> FECHADIAGNOSTICO { get; set; }
    
        public virtual INGRESO INGRESO { get; set; }
    }
}