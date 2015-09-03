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
    // sección de solicitudes de ingreso
    #region Models de solicitud de ingreso
        // 1. PERSONA.FICHA, esta entity (to model) es el punto de partida para una solicitud de ingreso
        public partial class FICHAMODEL
        {
            public FICHAMODEL()
            {
                this.INGRESO = new HashSet<INGRESO>();
            }

            [Key]
            public int ID { get; set; }

            //[Required]
            [Display(Name="NÚMERO DE IDENTIDAD")]
            [StringLength(15)]
            public string NUMEROIDENTIDAD { get; set; }

            //[Required]
            [Display(Name = "NÚMERO DE PASAPORTE")]
            [StringLength(15)]
            public string NUMEROPASAPORTE { get; set; }

            [Required]
            [Display(Name = "PRIMER NOMBRE")]
            [StringLength(50)]
            public string PRIMERNOMBRE { get; set; }

            
            [Display(Name = "SEGUNDO NOMBRE")]
            [StringLength(50)]
            public string SEGUNDONOMBRE { get; set; }

            [Required]
            [Display(Name = "PRIMER APELLIDO")]
            [StringLength(50)]
            public string PRIMERAPELLIDO { get; set; }

            [Display(Name = "SEGUNDO APELLIDO")]
            [StringLength(50)]
            public string SEGUNDOAPELLIDO { get; set; }

            [Display(Name = "NACIONALIDAD")]
            [StringLength(50)]
            public string NACIONALIDAD { get; set; }

            public virtual ICollection<INGRESO> INGRESO { get; set; }
            public virtual DATOSPERSONALES1 DATOSPERSONALES1 { get; set; }
        }      

        
        //2. PERSONA.DATOSPERSONALES, esta entity (to model) captura datos personales extra del solicitante
        public partial class DATOSPERSONALESFICHAMODEL
        {
            [Key]
            public int IDPERSONA { get; set; }

            [Display(Name = "DIRECCIÓN DOMICILIO")]
            public string DIRECCIONDOMICILIO { get; set; }

            [Display(Name = "TELEFONO FIJO")]
            public string TELEFONOFIJO { get; set; }

            [Display(Name = "TELEFONO MÓVIL")]
            public string TELEFONOMOVIL { get; set; }

            [Required]
            [Display(Name = "FECHA NACIMIENTO")]
            [DataType(DataType.Date)]
            public Nullable<System.DateTime> FECHANACIMIENTO { get; set; }

            [Display(Name = "ESTADO CIVIL")]
            public string ESTADOCIVIL { get; set; }

            [Display(Name = "NÚMERO DE HIJOS")]
            public Nullable<int> NUMERODEHIJOS { get; set; }

            [Display(Name = "NOMBRE MADRE")]
            [StringLength(50)]
            public string NOMBREMADRE { get; set; }

            [Display(Name = "VIVE MADRE?")]
            public bool VIVECONMADRE { get; set; }

            [Display(Name = "NOMBRE PADRE")]
            public string NOMBREPADRE { get; set; }

            [Display(Name = "VIVE PADRE?")]
            public bool VIVECONPADRE { get; set; }

            [Display(Name = "CON QUIÉN VIVE?")]
            [StringLength(50)]
            public string CONQUIENVIVE { get; set; }

            [Display(Name = "NÚMERO HERMANOS")]
            public Nullable<int> NUMEROHERMANOS { get; set; }

            [Display(Name = "PAIS NACIONALIDAD")]
            public string IDPAISNACIONALIDAD { get; set; }

            [Display(Name = "CIUDAD NATAL")]
            public Nullable<int> IDCIUDADNATAL { get; set; }

            [Display(Name = "PAIS RESIDENCIA ACTUAL")]
            public string IDPAISRESIDENCIAACTUAL { get; set; }

            [Display(Name = "CIUDAD RESIDENCIA ACTUAL")]
            public Nullable<int> IDCIUDADRESIDENCIAACTUAL { get; set; }

           

            public virtual CITY CITY { get; set; }
            public virtual CITY CITY1 { get; set; }
            public virtual COUNTRY COUNTRY { get; set; }
            public virtual FICHA FICHA { get; set; }

            // SET DE PAÍSES
            public virtual IEnumerable<COUNTRY> PAISES { get; set; }
        }


        //3. INGRESOINTERNO.INGRESO, esta entity (to model) es la columna vertebral de la rehabilitación de  un paciente, referencia absuluta a este ingreso
        public partial class INGRESOMODEL
        {
            public INGRESOMODEL()
            {
                this.APARATOSSISTEMAS = new HashSet<APARATOSSISTEMAS>();
                this.EVALUACIONMEDICADETALLE = new HashSet<EVALUACIONMEDICADETALLE>();
                this.EXAMENPSICOMETRICO_INTERNOCALIFICACIONES = new HashSet<EXAMENPSICOMETRICO_INTERNOCALIFICACIONES>();
                this.CAUSASEGRESOINTERNO = new HashSet<CAUSASEGRESOINTERNO>();
                this.CONDICIONFISICA_INTERNOENFERMEDADES = new HashSet<CONDICIONFISICA_INTERNOENFERMEDADES>();
                this.DATOSPROBLEMADROGAS_CONSUMODROGAS = new HashSet<DATOSPROBLEMADROGAS_CONSUMODROGAS>();
                this.DOCUMENTOSINGRESO = new HashSet<DOCUMENTOSINGRESO>();
                this.CENTRODESARROLLOINGRESO = new HashSet<CENTRODESARROLLOINGRESO>();
                this.FICHA = new FICHA();

                this.DROGAS = new HashSet<DATOSPROBLEMADROGAS_DROGAS>();
                this.ESCOLARIDADES = new HashSet<INFORMACIONACADEMICA_ESCOLARIDAD>();
                this.OFICIOS = new HashSet<INFORMACIONACADEMICA_OFICIOS>();
            }

            [Key]
            public int ID { get; set; }

            [Required]
            [Display(Name="NÚMERO EXPEDIENTE PV")]
            [StringLength(15)]
            public string NUMEXPEDIENTE { get; set; }

            [Display(Name = "FECHA AUTORIZACIÓN INGRESO")]
            [DataType(DataType.DateTime)]
            public Nullable<System.DateTime> FECHAUTORIZACION { get; set; }

            [Display(Name = "FECHA AUTORIZACIÓN INGRESO")]
            [DataType(DataType.DateTime)]
            public Nullable<System.DateTime> FECHAFIRMAACUERDO { get; set; }

            [Display(Name = "FECHA INGRESO CENTRO TERAPÉUTICO")]
            [DataType(DataType.DateTime)]
            public Nullable<System.DateTime> FECHAREALINGRESOPV { get; set; }

            [Display(Name = "FECHA SALIDA (EGRESO) CENTRO TERAPÉUTICO")]
            [DataType(DataType.DateTime)]
            public Nullable<System.DateTime> FECHAEGRESOPV { get; set; }

            // FECHA SISTEMA
            public Nullable<System.DateTime> FECHAINGRESOSISTEMA { get; set; }
            [Display(Name = "OBSERVACIONES")]
            public string OBSERVACIONES { get; set; }

            [Display(Name = "ARCHIVO FIRMA CONTRATO")]
            public string CONTRATO { get; set; }

            [HiddenInput(DisplayValue= false)]
            public Nullable<int> IDPERSONA { get; set; }
            
            [Display(Name = "¿ACEPTADO?")]
            public Nullable<bool> ACEPTADO { get; set; }

            [HiddenInput(DisplayValue = false)]
            public Nullable<int> STATUSFLOW { get; set; }

            [Required]
            [Display(Name="CENTRO TERAPEUTICO")]
            public string IDCRENTROTERAPEUTICO { get; set; }

            public virtual ICollection<APARATOSSISTEMAS> APARATOSSISTEMAS { get; set; }
            public virtual ICollection<EVALUACIONMEDICADETALLE> EVALUACIONMEDICADETALLE { get; set; }
            public virtual SIGNOSVITALES SIGNOSVITALES { get; set; }
            public virtual DATOSSOCIOECONOMICOS DATOSSOCIOECONOMICOS { get; set; }
            public virtual ESTADOMENTAL ESTADOMENTAL { get; set; }
            public virtual EXAMENPSICOMETRICO EXAMENPSICOMETRICO { get; set; }
            public virtual ICollection<EXAMENPSICOMETRICO_INTERNOCALIFICACIONES> EXAMENPSICOMETRICO_INTERNOCALIFICACIONES { get; set; }
            public virtual HISTORIALPERSONALFAMILIAR HISTORIALPERSONALFAMILIAR { get; set; }
            public virtual IMPRESIONDIAGNOSTICA IMPRESIONDIAGNOSTICA { get; set; }
            public virtual OBERVACIONPRELIMINAR OBERVACIONPRELIMINAR { get; set; }
            public virtual RECOMENDACIONES RECOMENDACIONES { get; set; }
            public virtual ICollection<CAUSASEGRESOINTERNO> CAUSASEGRESOINTERNO { get; set; }
            public virtual CONDICIONFISICA_INGRESO CONDICIONFISICA_INGRESO { get; set; }
            public virtual ICollection<CONDICIONFISICA_INTERNOENFERMEDADES> CONDICIONFISICA_INTERNOENFERMEDADES { get; set; }
            public virtual DATOSDELICTIVOS DATOSDELICTIVOS { get; set; }
            public virtual DATOSPROBLEMADROGAS_CONSUMO DATOSPROBLEMADROGAS_CONSUMO { get; set; }
            public virtual ICollection<DATOSPROBLEMADROGAS_CONSUMODROGAS> DATOSPROBLEMADROGAS_CONSUMODROGAS { get; set; }
            public virtual INFORMACIONACADEMICA_ESTUDIOS INFORMACIONACADEMICA_ESTUDIOS { get; set; }
            public virtual FICHA FICHA { get; set; }
            public virtual MOTIVOSINGRESO MOTIVOSINGRESO { get; set; }
            public virtual ICollection<DOCUMENTOSINGRESO> DOCUMENTOSINGRESO { get; set; }
            public virtual ICollection<CENTRODESARROLLOINGRESO> CENTRODESARROLLOINGRESO { get; set; }

            // model para drogas
            public int IDDROGA { get; set; }
            public virtual ICollection<DATOSPROBLEMADROGAS_DROGAS> DROGAS { get; set; }

            // model para estudios
            public int IDESCOLARIDAD { get; set; }
            public virtual ICollection<INFORMACIONACADEMICA_ESCOLARIDAD> ESCOLARIDADES { get; set; }

            // MODEL PARA OFICIOS
            public int IDOFICIO { get; set; }
            public virtual ICollection<INFORMACIONACADEMICA_OFICIOS> OFICIOS { get; set; }
        }
        
        //4. INGRESOINTERNO.DOCUMENTOS, esta enity, se encarga de relacionar los documentos que se presentan al momento de solicitar el ingreso
        public partial class DOCUMENTOSINGRESOVIEWMODEL
        {
            public virtual INGRESO INGRESO { get; set; }
            public virtual IEnumerable<TIPOSDOCUMENTO> TIPOSDOCUMENTOSINGRESOVIEWM { get; set; }
        }

        //5. INGRESOINTSERNO.CENTRODESAROLLOINGRESO, esta entity se relaciona al centro de desarrollo de la terapia de rehabilitación
        public partial class CENTRODESARROLLOINGRESOMODEL
        {
            [Key]
            public int ID { get; set; }
            [Required]
            [Display(Name="FECHA TRASLADO")]
            [DataType(DataType.DateTime)]
            public Nullable<System.DateTime> FECHATRASLADO { get; set; }

            [Display(Name = "ESTADO")]
            public bool ACTIVO { get; set; }

            //FECHA DE SISTEMA
            [HiddenInput(DisplayValue=false)]
            public Nullable<System.DateTime> FECHAREGISTRO { get; set; }

            [Required]
            [Display(Name = "CENTRO TERAPÉUTICO")]
            public string IDCENTROTERAPEUTICO { get; set; }

            [HiddenInput(DisplayValue = false)]
            public Nullable<int> IDINGRESO { get; set; }

            public virtual CENTROTERAPEUTICO CENTROTERAPEUTICO { get; set; }
            public virtual INGRESO INGRESO { get; set; }
        }

        // X. ViewModels --> es la co-relación entre los modelos declarados arriba-- 
        public partial class INGRESOSVIEWMODELS
        {
            public virtual IEnumerable<FICHAMODEL> FICHASVIEWMODELS { get; set; }
            public virtual IEnumerable<DATOSPERSONALESFICHAMODEL> DATSOPERSONALESFICHASVIEWMODEL { get; set; }
            public virtual IEnumerable<INGRESOMODEL> INGRESOVIEWMODEL { get; set; }
            public virtual IEnumerable<CENTRODESARROLLOINGRESOMODEL> CENTRODESARROLLOINGRESOVIEWMODEL { get; set; }
        }
    #endregion

    //  sección de evaluación médica
        #region Evaluación médica models

        public partial class EvaluacionMedicaViewModel
        {
            public INGRESO Ingreso { get; set; }
            public virtual IEnumerable<TIPOEVALUACIONMEDICA> TiposEvMedica { get; set; }
            public virtual IEnumerable<APARATOSSISTEMAS_SISTEMAS> AparatosSistemas { get; set; }
        }

        #endregion

}