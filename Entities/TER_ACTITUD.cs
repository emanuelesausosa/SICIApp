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
    
    public partial class TER_ACTITUD
    {
        public TER_ACTITUD()
        {
            this.TER_EVALUACIONCARACTER = new HashSet<TER_EVALUACIONCARACTER>();
        }
    
        public int ID { get; set; }
        public string NOMBRE { get; set; }
        public string DESCRIPCION { get; set; }
        public Nullable<decimal> CALIFICACIONESTIMADA { get; set; }
        public Nullable<int> IDCATEGORIAASPECTO { get; set; }
    
        public virtual TER_CATEGORIAASPECTOCARCTER TER_CATEGORIAASPECTOCARCTER { get; set; }
        public virtual ICollection<TER_EVALUACIONCARACTER> TER_EVALUACIONCARACTER { get; set; }
    }
}
