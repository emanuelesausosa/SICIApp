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
    
    public partial class EVALUACIONMEDICADETALLE
    {
        public int ID { get; set; }
        public Nullable<int> IDINGRESO { get; set; }
        public Nullable<int> IDTIPOEVALUACIONMEDICA { get; set; }
        public string OBSERVACIONES { get; set; }
        public Nullable<System.DateTime> FECHADIAGNOSTICO { get; set; }
    
        public virtual INGRESO INGRESO { get; set; }
        public virtual TIPOEVALUACIONMEDICA TIPOEVALUACIONMEDICA { get; set; }
    }
}
