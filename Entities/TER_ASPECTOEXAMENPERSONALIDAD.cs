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
    
    public partial class TER_ASPECTOEXAMENPERSONALIDAD
    {
        public int ID { get; set; }
        public Nullable<int> IDASPECTO { get; set; }
        public Nullable<int> IDEXAMEN { get; set; }
        public Nullable<decimal> CALIFICACION { get; set; }
        public Nullable<System.DateTime> FECHA { get; set; }
    
        public virtual TER_ASPECTOPERSONALIDAD TER_ASPECTOPERSONALIDAD { get; set; }
        public virtual TER_EXAMENPERSONALIDAD TER_EXAMENPERSONALIDAD { get; set; }
    }
}
