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
    
    public partial class CENTRODESARROLLOINGRESO
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> FECHATRASLADO { get; set; }
        public Nullable<bool> ACTIVO { get; set; }
        public Nullable<System.DateTime> FECHAREGISTRO { get; set; }
        public string IDCENTROTERAPEUTICO { get; set; }
        public Nullable<int> IDINGRESO { get; set; }
    
        public virtual CENTROTERAPEUTICO CENTROTERAPEUTICO { get; set; }
        public virtual INGRESO INGRESO { get; set; }
    }
}
