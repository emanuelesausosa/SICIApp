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
    #region CENTRO TERAPEUTICO -- MODEL
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
        [Display(Name = "CODIGO DEL CENTRO")]
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
    #endregion


    #region TIPO EVALUACION MEDICA --- MODEL
    public partial class TIPOEVALUACIONMEDICAMODEL
    {
        public TIPOEVALUACIONMEDICAMODEL()
        {
            this.EVALUACIONMEDICADETALLE = new HashSet<EVALUACIONMEDICADETALLE>();
        }

        [Key]
        [HiddenInput(DisplayValue= false)]
        public int ID { get; set; }

        [Required]
        [Display(Name="CATEGORÍA")]
        [StringLength(50)]
        public string CATEGORIA { get; set; }

        [Display(Name="DECRIPCIÓN")]
        public string DESCRIPCION { get; set; }

        public virtual ICollection<EVALUACIONMEDICADETALLE> EVALUACIONMEDICADETALLE { get; set; }
    } 
    #endregion


    // MODELO DE APARATOS SISTEMA - SISTEMAS
    #region APARATOS SISTEMAS, SISTEMAS -- MODEL

    public partial class APARATOSSISTEMAS_SISTEMASMODEL
    {
        public APARATOSSISTEMAS_SISTEMASMODEL()
        {
            this.APARATOSSISTEMAS = new HashSet<APARATOSSISTEMAS>();
        }

        [Key]
        [HiddenInput(DisplayValue= false)]
        public int ID { get; set; }

        [Required]
        [Display(Name="NOMBRE")]
        [StringLength(50)]
        public string NOMBRE { get; set; }

        [Display(Name="DESCRIPCIÓN")]
        public string DESCRIPCION { get; set; }

        public virtual ICollection<APARATOSSISTEMAS> APARATOSSISTEMAS { get; set; }
    }

    #endregion

    #region TIPOS DOCUMENTOS -- MODEL

    public partial class TIPOSDOCUMENTOMODEL
    {
        public TIPOSDOCUMENTOMODEL()
        {
            this.DOCUMENTOSINGRESO = new HashSet<DOCUMENTOSINGRESO>();
        }

        [Key]
        [HiddenInput(DisplayValue= false)]
        public int ID { get; set; }

        [Required]
        [Display(Name="NOMBRE")]
        [StringLength(50)]
        public string NOMBRETIPO { get; set; }

        [Display(Name="DESCRIPCIÓN")]
        public string DESCRIPCION { get; set; }

        public virtual ICollection<DOCUMENTOSINGRESO> DOCUMENTOSINGRESO { get; set; }
    }

    #endregion

    // CAUSAS EGRESO - MODEL
    #region CAUSAS EGRESO -- MODEL
    public partial class CAUSASEGRESOMODEL
    {
        public CAUSASEGRESOMODEL()
        {
            this.CAUSASEGRESOINTERNO = new HashSet<CAUSASEGRESOINTERNO>();
        }

        [Key]
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [Required]
        [Display(Name = "NOMBRE")]
        [StringLength(50)]
        public string NOMBRE { get; set; }

        [Display(Name = "DESCRIPCIÓN")]
        public string DESCRIPCION { get; set; }

        public virtual ICollection<CAUSASEGRESOINTERNO> CAUSASEGRESOINTERNO { get; set; }
    } 
    #endregion

    //  DROGAS
    #region DATOS PROBLEMAS DROGAS -- DROGAS
    public partial class DATOSPROBLEMADROGAS_DROGASMODEL
    {
        public DATOSPROBLEMADROGAS_DROGASMODEL()
        {
            this.DATOSPROBLEMADROGAS_CONSUMODROGAS = new HashSet<DATOSPROBLEMADROGAS_CONSUMODROGAS>();
        }

        [Key]
        [HiddenInput(DisplayValue=false)]
        public int ID { get; set; }

        [Required]
        [Display(Name="NOMBRE CIENTÍFICO")]
        [StringLength(50)]
        public string NOMBRECIENTIFICO { get; set; }

        [Required]
        [Display(Name = "NOMBRE COMÚN")]
        [StringLength(50)]
        public string NOMBRECOMUN { get; set; }

        [Display(Name = "DESCRIPCIÓN")]
        public string DESCRIPCION { get; set; }

        public virtual ICollection<DATOSPROBLEMADROGAS_CONSUMODROGAS> DATOSPROBLEMADROGAS_CONSUMODROGAS { get; set; }
    }
    #endregion

    // EXÁMENES PS-MÉTRICOS
    #region EXAMEN PSICOMÉTRICO
    public partial class EXAMENPSICOMETRICO_EXAMENMODEL
    {
        public EXAMENPSICOMETRICO_EXAMENMODEL()
        {
            this.EXAMENPSICOMETRICO_EXAMENCONTENIDO = new HashSet<EXAMENPSICOMETRICO_EXAMENCONTENIDO>();
        }
    
        [Key]
        [HiddenInput(DisplayValue=false)]
        public int IDEXAMEN { get; set; }

        [Required]
        [Display(Name="NOMBRE")]
        [StringLength(50)]
        public string TITULO { get; set; }

        [Display(Name = "DESCRIPCIÓN")]
        public string DESCRIPCION { get; set; }

        [Display(Name = "ARCHIVO")]
        public string ARCHIVOFISOCO { get; set; }
    
        public virtual ICollection<EXAMENPSICOMETRICO_EXAMENCONTENIDO> EXAMENPSICOMETRICO_EXAMENCONTENIDO { get; set; }
    }
    #endregion

    #region EXAMEN PS-METRTICO - CONTENIDO - MODEL
    public partial class EXAMENPSICOMETRICO_EXAMENCONTENIDOMODEL
    {
        public EXAMENPSICOMETRICO_EXAMENCONTENIDOMODEL()
        {
            this.EXAMENPSICOMETRICO_INTERNOCALIFICACIONES = new HashSet<EXAMENPSICOMETRICO_INTERNOCALIFICACIONES>();
        }

        [Key]
        [HiddenInput(DisplayValue=false)]
        public int IDPREGUNTA { get; set; }

        [Required]
        [Display(Name="DETALLE")]
        [StringLength(150)]
        public string DETALLE { get; set; }

        [Required]
        [Display(Name = "VALORACIÓN")]
        public Nullable<int> VALORACION { get; set; }

        
        [Display(Name = "DESCRIPCIÓN")]
        [StringLength(150)]
        public string DESCRIPCION { get; set; }

        [Display(Name="EXAMEN")]
        public Nullable<int> IDEXAMEN { get; set; }

        public virtual EXAMENPSICOMETRICO_EXAMEN EXAMENPSICOMETRICO_EXAMEN { get; set; }
        public virtual ICollection<EXAMENPSICOMETRICO_INTERNOCALIFICACIONES> EXAMENPSICOMETRICO_INTERNOCALIFICACIONES { get; set; }
    }
    #endregion

    //ENFERMDAES -- MODEL
    #region CONDICIÓN FÍSICA -- ENFERMEDADES -- MODEL
    public partial class CONDICIONFISICA_ENFERMEDADESMODEL
    {
        public CONDICIONFISICA_ENFERMEDADESMODEL()
        {
            this.CONDICIONFISICA_INTERNOENFERMEDADES = new HashSet<CONDICIONFISICA_INTERNOENFERMEDADES>();
        }
    
        [Key]
        [HiddenInput(DisplayValue= false)]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name="NOMBRE CIENTÍFICO")]
        public string NOMBRECIENTIFICO { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "NOMBRE COMÚN")]
        public string NOMBRESINOMIMO { get; set; }

        [Display(Name = "DESCRIPCIÓN")]
        public string DESCRIPCION { get; set; }
    
        public virtual ICollection<CONDICIONFISICA_INTERNOENFERMEDADES> CONDICIONFISICA_INTERNOENFERMEDADES { get; set; }
    }
    #endregion


    // INFOMACIÓNJ ACADÉMICA -- ESCOLARIDAD

    #region ESCOLARIDAD -- MODEL

    public partial class INFORMACIONACADEMICA_ESCOLARIDADMODEL
    {
        public INFORMACIONACADEMICA_ESCOLARIDADMODEL()
        {
            this.INFORMACIONACADEMICA_ESTUDIOSESCOLARIDAD = new HashSet<INFORMACIONACADEMICA_ESTUDIOSESCOLARIDAD>();
        }

        [Key]
        [HiddenInput(DisplayValue=false)]
        public int ID { get; set; }

        [Required]
        [Display(Name="TÍTULO")]
        [StringLength(50)]
        public string NOMBREESCOLARIDAD { get; set; }

        [Display(Name = "DESCRIPCIÓN")]
        public string DESCRIPCIONESCOLARIDAD { get; set; }

        public virtual ICollection<INFORMACIONACADEMICA_ESTUDIOSESCOLARIDAD> INFORMACIONACADEMICA_ESTUDIOSESCOLARIDAD { get; set; }
    }
    #endregion

    // ESTUDIOS -- ESCOLARIDAD MODEL
    
    #region eSTUDIOS ESCOLARIDAD -- MODEL

    public partial class INFORMACIONACADEMICA_ESTUDIOSESCOLARIDAD
    {
        [Key]
        [HiddenInput(DisplayValue=false)]
        public int ID { get; set; }

        [Required]
        [Display(Name="ESTUDIO")]
        public Nullable<int> IDESTUDIOS { get; set; }

        [Required]
        [Display(Name = "ESCOLARIDAD")]
        public Nullable<int> IDESCOLARIDAD { get; set; }

        public virtual INFORMACIONACADEMICA_ESCOLARIDAD INFORMACIONACADEMICA_ESCOLARIDAD { get; set; }
        public virtual INFORMACIONACADEMICA_ESTUDIOS INFORMACIONACADEMICA_ESTUDIOS { get; set; }
    }
    #endregion


    // OFICIOS -- MODEL
    #region OFICIOS -- MODEL
    public partial class INFORMACIONACADEMICA_OFICIOSMODEL
    {
        public INFORMACIONACADEMICA_OFICIOSMODEL()
        {
            this.INFORMACIONACADEMICA_ESTUDIOSOFICIO = new HashSet<INFORMACIONACADEMICA_ESTUDIOSOFICIO>();
        }

        [Key]
        [HiddenInput(DisplayValue=false)]
        public int ID { get; set; }

        [Required]
        [Display(Name="NOMBRE")]
        [StringLength(50)]
        public string NOMBREOFICIO { get; set; }

        [Display(Name = "DESCRIPCIÓN")]
        public string DESCRIPCIONOFICIO { get; set; }

        public virtual ICollection<INFORMACIONACADEMICA_ESTUDIOSOFICIO> INFORMACIONACADEMICA_ESTUDIOSOFICIO { get; set; }
    }
    #endregion
}