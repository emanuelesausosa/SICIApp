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
    
    public partial class INFORMACIONACADEMICA_ESTUDIOSESCOLARIDAD
    {
        public int ID { get; set; }
        public Nullable<int> IDESTUDIOS { get; set; }
        public Nullable<int> IDESCOLARIDAD { get; set; }
    
        public virtual INFORMACIONACADEMICA_ESCOLARIDAD INFORMACIONACADEMICA_ESCOLARIDAD { get; set; }
        public virtual INFORMACIONACADEMICA_ESTUDIOS INFORMACIONACADEMICA_ESTUDIOS { get; set; }
    }
}