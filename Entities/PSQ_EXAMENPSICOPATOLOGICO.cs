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
    
    public partial class PSQ_EXAMENPSICOPATOLOGICO
    {
        public int IDINGRESO { get; set; }
        public Nullable<bool> COOPERA { get; set; }
        public string DETALLECOOPERA { get; set; }
        public string TEP { get; set; }
        public string LENGUAJE { get; set; }
        public string DESORDENPENSAMIENTO { get; set; }
        public string INTELECTO { get; set; }
        public string AFECTO { get; set; }
        public string AUTOCOMPRENSION { get; set; }
        public string NIVELDROGADICCION { get; set; }
        public string OTROS { get; set; }
        public string NEUROLOGICOS { get; set; }
        public string DIAGNOSTICO { get; set; }
        public string RECOMENDACIONES { get; set; }
        public string TRATAMIENTO { get; set; }
        public Nullable<System.DateTime> FECHADIAGNOSTICO { get; set; }
    
        public virtual INGRESO INGRESO { get; set; }
    }
}
