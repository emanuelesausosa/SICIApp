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
    
    public partial class TER_CURSO
    {
        public TER_CURSO()
        {
            this.TER_MATRICULA = new HashSet<TER_MATRICULA>();
        }
    
        public int ID { get; set; }
        public string NOMBRE { get; set; }
        public string DESCRIPCION { get; set; }
    
        public virtual ICollection<TER_MATRICULA> TER_MATRICULA { get; set; }
    }
}