using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;
using SICIApp.Dominio;
using SICIApp.Entities;

namespace SICIApp.Models
{
    public partial class CENTROTERAPEUTICOMODEL
    {
        public CENTROTERAPEUTICOMODEL()
        {
            this.USUARIOCENTROCONSULTAs = new HashSet<USUARIOCENTROCONSULTA>();
            this.CITIES = new HashSet<CITY>();
        }

        //es la referencia para el filtro de elección de la ciudad
        public string CODIGOPAIS { get; set; }
        
        [Required]
        [Display(Name="CODIGO DEL CENTRO")]
        [StringLength(10)]
        public string ID { get; set; }

        [Required]
        [Display(Name = "NOMBRE DEL CENTRO")]
        [StringLength(50)]
        public string NOMBRE { get; set; }

        
        [Display(Name = "DESCRIPCIÓN")]
        public string DESCRIPCION { get; set; }
        public Nullable<int> IDCIUDADOPERACION { get; set; }

        [Required]
        [Display(Name = "CIUDAD DE OPERACION")]
        public int IDCIUDAD { get; set; }

        public virtual CITY CITY { get; set; }
        public virtual ICollection<USUARIOCENTROCONSULTA> USUARIOCENTROCONSULTAs { get; set; }
        public virtual IEnumerable<CITY> CITIES { get; set; }

    }
}