using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Web.Mvc;
using SICIApp.Dominio;
using SICIApp.Entities;


namespace SICIApp.Models
{
    #region Talonario Model
    public partial class CONT_TALONARIOMODEL
    {
        public CONT_TALONARIOMODEL()
        {
            this.CONT_CODOTALONARIO = new HashSet<CONT_CODOTALONARIO>();
        }

        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "FECHA CREADO")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> FECHACREADO { get; set; }

        [Required]
        [Display(Name = "ESTADO")]
        public string ESTADO { get; set; }

        [Display(Name = "DESCRIPCIÓN")]
        public string DESCRIPCION { get; set; }


        [Display(Name = "FECHA CANCELADO")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> FECHACANCELADO { get; set; }

        [Required]
        [Display(Name = "IDINGRESO")]
        public Nullable<int> IDINGRESO { get; set; }

        public virtual ICollection<CONT_CODOTALONARIO> CONT_CODOTALONARIO { get; set; }
        public virtual INGRESO INGRESO { get; set; }
    } 
    #endregion

    #region Codo-Talonario Model
    public partial class CONT_CODOTALONARIOMODEL
    {
        public CONT_CODOTALONARIOMODEL()
        {
            this.CONT_FACTURA = new HashSet<CONT_FACTURA>();
        }
    
        [Key]
        public int ID { get; set; }

        [Display(Name="FECHA CREADO")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> FECHACREADO { get; set; }

        [Display(Name = "FECHA A PAGAR")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> FECHAAPAGAR { get; set; }

        [Display(Name = "FECHA PAGO")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> FECHAPAGO { get; set; }

        [Display(Name = "VALOR A PAGAR")]
        [DataType(DataType.Currency)]
        public Nullable<decimal> VALORAPAGAR { get; set; }

        [Display(Name = "DESCRIPCIÓN")]
        [DataType(DataType.MultilineText)]
        public string DESCRIPCION { get; set; }

        [Required]
        [Display(Name = "ID TALONARIO")]
        public Nullable<int> IDTALONARIO { get; set; }

        [Required]
        [Display(Name = "MES")]
        public Nullable<int> IDMES { get; set; }

        [Required]
        [Display(Name = "CONCEPTO")]
        public Nullable<int> IDTIPOCONCEPTO { get; set; }

        [Required]
        [Display(Name = "ESTADO")]
        public Nullable<int> IDTIPOESTADO { get; set; }
    
        public virtual CONT_ESTADOPAGO CONT_ESTADOPAGO { get; set; }
        public virtual CONT_MES CONT_MES { get; set; }
        public virtual CONT_TALONARIO CONT_TALONARIO { get; set; }
        public virtual CONT_TIPOCONCEPTOPAGO CONT_TIPOCONCEPTOPAGO { get; set; }
        public virtual ICollection<CONT_FACTURA> CONT_FACTURA { get; set; }
    }
    #endregion

    #region FACTURA MODEL
     public partial class CONT_FACTURAMODEL
    {
        public int ID { get; set; }
        public string NUMEROENFISICO { get; set; }
        public Nullable<System.DateTime> FECHACREACION { get; set; }
        public Nullable<decimal> VALORPAGADO { get; set; }
        public string DESCRIPCION { get; set; }
        public string ESTADO { get; set; }
        public string ARCHIVO { get; set; }
        public Nullable<int> IDCODO { get; set; }
    
        public virtual CONT_CODOTALONARIO CONT_CODOTALONARIO { get; set; }
    }
    #endregion
}