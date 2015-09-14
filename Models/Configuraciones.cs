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
            this.COUNTRIES = new HashSet<COUNTRY>();
        }

        //es la referencia para el filtro de elección de la ciudad
        

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

        [Display(Name = "PAÍS")]
        public string CODIGOPAIS { get; set; }

        [Required]
        [Display(Name = "CIUDAD DE OPERACION")]
        public Nullable<int> IDCIUDADOPERACION { get; set; }

        public virtual CITY CITY { get; set; }
        public virtual ICollection<USUARIOCENTROCONSULTA> USUARIOCENTROCONSULTAs { get; set; }
        public virtual IEnumerable<CITY> CITIES { get; set; }
        public virtual IEnumerable<COUNTRY> COUNTRIES { get; set; }

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
            this.INFORMACIONACADEMICA_ESTUDIOSESCOLARIDAD = new HashSet<INFORMACIONACADEMICA_ESTUDIOSESCOLARIDADMODEL>();
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

        public virtual ICollection<INFORMACIONACADEMICA_ESTUDIOSESCOLARIDADMODEL> INFORMACIONACADEMICA_ESTUDIOSESCOLARIDAD { get; set; }
    }
    #endregion

    // ESTUDIOS -- ESCOLARIDAD MODEL
    
    #region eSTUDIOS ESCOLARIDAD -- MODEL

    public partial class INFORMACIONACADEMICA_ESTUDIOSESCOLARIDADMODEL
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

    // ******************************************
    //----***MODELOS PARA MANTENIMIENTO DE RHABILITACIÓN***-----
    //**********************************************

    // FASE MODEL
    #region FASE MODEL
    public partial class PRO_FASEMODEL
    {
        public PRO_FASEMODEL()
        {
            this.PRO_NIVEL = new HashSet<PRO_NIVEL>();
        }

        [Key]        
        public int ID { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name="DESCRIPCIÓN")]
        public string DESCRIPCION { get; set; }

        [Display(Name="NOMBRE")]
        [Required(ErrorMessage = "Se requiere un nombre para crear una FASE")]
        [StringLength(50)]
        public string NOMBRE { get; set; }

        public virtual ICollection<PRO_NIVEL> PRO_NIVEL { get; set; }
    }

    #endregion

    #region NIVEL MODEL
     public partial class PRO_NIVELMODEL
    {
        public PRO_NIVELMODEL()
        {
            this.PRO_NIVELCRITERIO = new HashSet<PRO_NIVELCRITERIO>();
            this.PRO_PROMOCIONNIVEL = new HashSet<PRO_PROMOCIONNIVEL>();
        }
    
        [Key]
        public int ID { get; set; }
        
        [Display(Name="NOMBRE")]
        [StringLength(50)]
        [Required(ErrorMessage="Se requiere un nombre para crear un NIVEL")]
        public string NOMBRE { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name="DESCRIPCIÓN")]
        public string DESCRIPCION { get; set; }

        [Display(Name="DÍAS ESTIMADOS")]        
        [Range(1, 9999, ErrorMessage="Debe elegir un número entre 1 y 9999")]
        public Nullable<int> DIASESTIMADOS { get; set; }

        [Display(Name="FASE")]
        public Nullable<int> IDFASE { get; set; }
    
        public virtual PRO_FASE PRO_FASE { get; set; }
        public virtual ICollection<PRO_NIVELCRITERIO> PRO_NIVELCRITERIO { get; set; }
        public virtual ICollection<PRO_PROMOCIONNIVEL> PRO_PROMOCIONNIVEL { get; set; }
    }
    #endregion

    #region CRITERIO MODEL
      public partial class PRO_CRITERIOMODEL
    {
        public PRO_CRITERIOMODEL()
        {
            this.PRO_NIVELCRITERIO = new HashSet<PRO_NIVELCRITERIO>();
        }
    
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage="Se requiere un nombre para identificar el criterio")]
        [Display(Name="NOMBRE")]
        [StringLength(50, ErrorMessage="Debe de escribir un nombre de máximo 50 caracteres")]
        public string NOMBRE { get; set; }

        [Display(Name="DESCRIPCIÓN")]
        public string DESCRIPCION { get; set; }
    
        public virtual ICollection<PRO_NIVELCRITERIO> PRO_NIVELCRITERIO { get; set; }
    }
     #endregion

      #region CATEGORIA FALTA


      public partial class TER_CATEGORIAFALTAMODEL
      {
          public TER_CATEGORIAFALTAMODEL()
          {
              this.TER_FALTA = new HashSet<TER_FALTA>();
          }

          [Key]
          public int ID { get; set; }

          [Display(Name="NOMBRE")]
          [Required(ErrorMessage="Un nombre es requerido para agreagr una CATEGORIA DE FALTA")]
          [StringLength(50, ErrorMessage="El nombre de CATEGORIA FALTA debe de ser de un máximo de 50 caracteres")]
          public string NOMBRE { get; set; }

          [Display(Name="DESCRIPCIÓN")]
          [DataType(DataType.MultilineText)]
          public string DESCRIPCION { get; set; }

          public virtual ICollection<TER_FALTA> TER_FALTA { get; set; }
      }
        
      #endregion

      #region CATEGORIA PERSONALIDAD
      public partial class TER_CATEGORIAPERSONALIDADMODEL
    {
        public TER_CATEGORIAPERSONALIDADMODEL()
        {
            this.TER_ASPECTOPERSONALIDAD = new HashSet<TER_ASPECTOPERSONALIDAD>();
        }
    
        [Key]
          public int ID { get; set; }

          [Display(Name="NOMBRE")]
          [Required(ErrorMessage="Un nombre es requerido para agreagr una CATEGORIA DE PERSONALIDAD")]
          [StringLength(50, ErrorMessage="El nombre de CATEGORIA PERSONALIDAD debe de ser de un máximo de 50 caracteres")]
          public string NOMBRE { get; set; }

          [Display(Name="DESCRIPCIÓN")]
          [DataType(DataType.MultilineText)]
          public string DESCRIPCION { get; set; }
    
        public virtual ICollection<TER_ASPECTOPERSONALIDAD> TER_ASPECTOPERSONALIDAD { get; set; }
    }
      #endregion

      #region ASPECTO PERSONALIDAD

      public partial class TER_ASPECTOPERSONALIDADMODEL
      {
          public TER_ASPECTOPERSONALIDADMODEL()
          {
              this.TER_ASPECTOEXAMENPERSONALIDAD = new HashSet<TER_ASPECTOEXAMENPERSONALIDAD>();
          }

          [Key]
          public int ID { get; set; }

          [Display(Name = "NOMBRE")]
          [Required(ErrorMessage = "Un nombre es requerido para agreagr un ASPECTO PERSONALIDAD")]
          [StringLength(50, ErrorMessage = "El nombre de ASPECTO PERSONALIDAD debe de ser de un máximo de 50 caracteres")]
          public string NOMBRE { get; set; }

          [Display(Name = "DESCRIPCIÓN")]
          [DataType(DataType.MultilineText)]
          public string DESCRIPCION { get; set; }
                    
          [Display(Name="CALIFICACIÓN ESPERADA")]
          [Range(1, 100, ErrorMessage = "Debe elegir un valor entre 1 y 100")]
          public Nullable<int> CALIFICACIONESPERADA { get; set; }

          [Display(Name="CATEGORIA PERSONALIDAD")]
          public Nullable<int> IDCATEGORIAPERSONALIDAD { get; set; }

          public virtual ICollection<TER_ASPECTOEXAMENPERSONALIDAD> TER_ASPECTOEXAMENPERSONALIDAD { get; set; }
          public virtual TER_CATEGORIAPERSONALIDAD TER_CATEGORIAPERSONALIDAD { get; set; }
      }
      #endregion

      #region CATEGORIA ASPECTO CARACTER  -- MODEL
      public partial class TER_CATEGORIAASPECTOCARCTERMODEL
      {
          public TER_CATEGORIAASPECTOCARCTERMODEL()
          {
              this.TER_ACTITUD = new HashSet<TER_ACTITUD>();
          }

          [Key]
          public int ID { get; set; }

          [Display(Name = "NOMBRE")]
          [Required(ErrorMessage = "Un nombre es requerido para agreagr una CATEGORIA ASPECTO-CARCTER ")]
          [StringLength(50, ErrorMessage = "El nombre de CATEGORIA ASPECTO-CARCTER debe de ser de un máximo de 50 caracteres")]
          public string NOMBRE { get; set; }

          [Display(Name = "DESCRIPCIÓN")]
          [DataType(DataType.MultilineText)]
          public string DESCRIPCION { get; set; }

          public virtual ICollection<TER_ACTITUD> TER_ACTITUD { get; set; }
      }
      #endregion

      #region ACTIUD MODEL
      public partial class TER_ACTITUDMODEL
      {
          public TER_ACTITUDMODEL()
          {
              this.TER_EVALUACIONCARACTER = new HashSet<TER_EVALUACIONCARACTER>();
          }

          [Key]
          public int ID { get; set; }

          [Display(Name = "NOMBRE")]
          [Required(ErrorMessage = "Un nombre es requerido para agreagr una ACTITUD ")]
          [StringLength(50, ErrorMessage = "El nombre de ACTITUD debe de ser de un máximo de 50 caracteres")]
          public string NOMBRE { get; set; }

          [Display(Name = "DESCRIPCIÓN")]
          [DataType(DataType.MultilineText)]
          public string DESCRIPCION { get; set; }
          
          [Display(Name = "CALIFICACIÓN ESPERADA")]
          [Range(1, 100, ErrorMessage = "Debe elegir un valor entre 1 y 100")]
          public Nullable<decimal> CALIFICACIONESTIMADA { get; set; }

          [Display(Name="CATEGORIA ASPECTO")]
          public Nullable<int> IDCATEGORIAASPECTO { get; set; }

          public virtual TER_CATEGORIAASPECTOCARCTER TER_CATEGORIAASPECTOCARCTER { get; set; }
          public virtual ICollection<TER_EVALUACIONCARACTER> TER_EVALUACIONCARACTER { get; set; }
      }
      #endregion

      #region INSTRUCTOR  MODEL
      public partial class TER_INSTRUCTORMODEL
    {
        public TER_INSTRUCTORMODEL()
        {
            this.TER_MATRICULA = new HashSet<TER_MATRICULA>();
        }
    
        [Key]
        public int ID { get; set; }

        [Display(Name = "NOMBRE")]
        [Required(ErrorMessage = "Un nombre es requerido para agreagr un INSTRUCTOR ")]
        [StringLength(50, ErrorMessage = "El nombre de INSTRUCTOR debe de ser de un máximo de 50 caracteres")]
        public string NOMBRE { get; set; }

        [Display(Name="INSTITUCIÓN")]
        [Required(ErrorMessage = "Un nombre es requerido para agreagr una INSTITUCIÓN ")]
        [StringLength(50, ErrorMessage = "El nombre de INSTITUCIÓN debe de ser de un máximo de 50 caracteres")]
        public string INSTITUCION { get; set; }
    
        public virtual ICollection<TER_MATRICULA> TER_MATRICULA { get; set; }
    }
      #endregion

      #region CURSO MODEL

      public partial class TER_CURSOMODEL
      {
          public TER_CURSOMODEL()
          {
              this.TER_MATRICULA = new HashSet<TER_MATRICULA>();
          }

          [Key]
          public int ID { get; set; }

          [Display(Name = "NOMBRE")]
          [Required(ErrorMessage = "Un nombre es requerido para agreagr un CURSO ")]
          [StringLength(50, ErrorMessage = "El nombre de CURSO debe de ser de un máximo de 50 caracteres")]
          public string NOMBRE { get; set; }

          [Display(Name = "DESCRIPCIÓN")]
          [DataType(DataType.MultilineText)]
          public string DESCRIPCION { get; set; }

          public virtual ICollection<TER_MATRICULA> TER_MATRICULA { get; set; }
      }
      #endregion

      #region CABANIA - MODEL
       public partial class PRO_CABANIAMODEL
    {
        public PRO_CABANIAMODEL()
        {
            this.PRO_CABANIADESARROLLO = new HashSet<PRO_CABANIADESARROLLO>();
        }
    
        [Key]
        public int ID { get; set; }

        [Display(Name="NOMBRE")]
        [Required]
        [StringLength(50, ErrorMessage="El nombre debe de ser de un máximo de 50 caracteres")]
        public string NOMBRE { get; set; }

        [Display(Name="DESCRIPCIÓN")]
        [DataType(DataType.MultilineText)]
        public string DESCRIPCION { get; set; }

        [Display(Name="CAPACIDAD")]
        [Required]
        [Range(1,99, ErrorMessage="Debe de establecer un número entre 1 y 99")]
        public Nullable<int> CAPACIDAD { get; set; }


        [HiddenInput(DisplayValue = false)]
        public Nullable<double> GEOLAT { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Nullable<double> GEOLONG { get; set; }

        [Display(Name="ACTIVO")]
        public Nullable<bool> ACTIVO { get; set; }

        [Display(Name="CENTRO TERAPÉUTICO")]
        public string IDCENTROTERAPEUTICO { get; set; }

        [Display(Name="NIVEL")]
        public Nullable<int> IDNIVEL { get; set; }

        [HiddenInput(DisplayValue=false)]
        public string USUARIO { get; set; }
    
        public virtual CENTROTERAPEUTICO CENTROTERAPEUTICO { get; set; }
        public virtual PRO_NIVEL PRO_NIVEL { get; set; }
        public virtual ICollection<PRO_CABANIADESARROLLO> PRO_CABANIADESARROLLO { get; set; }
    }
      #endregion

    //
    ///*****************SECCIÓN DE CONFIGURACIONES DE CONTABILIDADES **********/////
    ///****************************************************************************
    ///

       #region ESTADOPAGO -MODEL
       public partial class CONT_ESTADOPAGOMODEL
       {
           public CONT_ESTADOPAGOMODEL()
           {
               this.CONT_CODOTALONARIO = new HashSet<CONT_CODOTALONARIO>();
           }
           
           [Key]
           public int ID { get; set; }

           [Display(Name = "NOMBRE")]
           [Required]
           [StringLength(25, ErrorMessage = "El nombre debe de ser de un máximo de 25 caracteres")]
           public string NOMBRE { get; set; }

           [Display(Name = "DESCRIPCIÓN")]
           [DataType(DataType.MultilineText)]
           public string DESCRIPCION { get; set; }


           public virtual ICollection<CONT_CODOTALONARIO> CONT_CODOTALONARIO { get; set; }
       }
       #endregion

       #region TIPOCONCEPTO PAGP - MODEL
       public partial class CONT_TIPOCONCEPTOPAGOMODEL
    {
        public CONT_TIPOCONCEPTOPAGOMODEL()
        {
            this.CONT_CODOTALONARIO = new HashSet<CONT_CODOTALONARIO>();
        }
    
           [Key]
           public int ID { get; set; }

           [Display(Name = "NOMBRE")]
           [Required]
           [StringLength(25, ErrorMessage = "El nombre debe de ser de un máximo de 25 caracteres")]
           public string NOMBRE { get; set; }

           [Display(Name = "DESCRIPCIÓN")]
           [DataType(DataType.MultilineText)]
           public string DESCRIPCION { get; set; }
    
        public virtual ICollection<CONT_CODOTALONARIO> CONT_CODOTALONARIO { get; set; }
    }
       #endregion

       #region MES - MODEL
       public partial class CONT_MESMODEL
    {
        public CONT_MESMODEL()
        {
            this.CONT_CODOTALONARIO = new HashSet<CONT_CODOTALONARIO>();
        }

        [Key]
        public int ID { get; set; }

        [Display(Name = "NOMBRE")]
        [Required]
        [StringLength(25, ErrorMessage = "El nombre debe de ser de un máximo de 25 caracteres")]
        public string NOMBREMES { get; set; }

        [Display(Name = "FECHA INICIO MES")]
        [Required]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> FECHAINICIO { get; set; }

        [Display(Name = "FECHA FIN DE MES")]
        [Required]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> FECHACIERRE { get; set; }

        [Display(Name = "FECHA CORTE")]
        [Required]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> FECHACORTE { get; set; }

        [Display(Name = "DESCRIPCIÓN")]
        [DataType(DataType.MultilineText)]
        public string DESCRIPCION { get; set; }
    
        public virtual ICollection<CONT_CODOTALONARIO> CONT_CODOTALONARIO { get; set; }
    }
       #endregion

}