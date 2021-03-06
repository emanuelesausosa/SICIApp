﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class SICIBD2Entities1 : DbContext
    {
        public SICIBD2Entities1()
            : base("name=SICIBD2Entities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<CITY> CITies { get; set; }
        public virtual DbSet<COUNTRY> COUNTRies { get; set; }
        public virtual DbSet<COUNTRYLANGUAGE> COUNTRYLANGUAGEs { get; set; }
        public virtual DbSet<DATOSPERSONALE> DATOSPERSONALES { get; set; }
        public virtual DbSet<CENTROTERAPEUTICO> CENTROTERAPEUTICOes { get; set; }
        public virtual DbSet<USUARIOCENTROCONSULTA> USUARIOCENTROCONSULTAs { get; set; }
        public virtual DbSet<APARATOSSISTEMAS> APARATOSSISTEMAS { get; set; }
        public virtual DbSet<APARATOSSISTEMAS_SISTEMAS> APARATOSSISTEMAS_SISTEMAS { get; set; }
        public virtual DbSet<EVALUACIONMEDICADETALLE> EVALUACIONMEDICADETALLE { get; set; }
        public virtual DbSet<SIGNOSVITALES> SIGNOSVITALES { get; set; }
        public virtual DbSet<TIPOEVALUACIONMEDICA> TIPOEVALUACIONMEDICA { get; set; }
        public virtual DbSet<DATOSSOCIOECONOMICOS> DATOSSOCIOECONOMICOS { get; set; }
        public virtual DbSet<ESTADOMENTAL> ESTADOMENTAL { get; set; }
        public virtual DbSet<EXAMENPSICOMETRICO> EXAMENPSICOMETRICO { get; set; }
        public virtual DbSet<EXAMENPSICOMETRICO_EXAMEN> EXAMENPSICOMETRICO_EXAMEN { get; set; }
        public virtual DbSet<EXAMENPSICOMETRICO_EXAMENCONTENIDO> EXAMENPSICOMETRICO_EXAMENCONTENIDO { get; set; }
        public virtual DbSet<EXAMENPSICOMETRICO_INTERNOCALIFICACIONES> EXAMENPSICOMETRICO_INTERNOCALIFICACIONES { get; set; }
        public virtual DbSet<HISTORIALPERSONALFAMILIAR> HISTORIALPERSONALFAMILIAR { get; set; }
        public virtual DbSet<IMPRESIONDIAGNOSTICA> IMPRESIONDIAGNOSTICA { get; set; }
        public virtual DbSet<OBERVACIONPRELIMINAR> OBERVACIONPRELIMINAR { get; set; }
        public virtual DbSet<RECOMENDACIONES> RECOMENDACIONES { get; set; }
        public virtual DbSet<CAUSASEGRESO> CAUSASEGRESO { get; set; }
        public virtual DbSet<CAUSASEGRESOINTERNO> CAUSASEGRESOINTERNO { get; set; }
        public virtual DbSet<CONDICIONFISICA_ENFERMEDADES> CONDICIONFISICA_ENFERMEDADES { get; set; }
        public virtual DbSet<CONDICIONFISICA_INGRESO> CONDICIONFISICA_INGRESO { get; set; }
        public virtual DbSet<CONDICIONFISICA_INTERNOENFERMEDADES> CONDICIONFISICA_INTERNOENFERMEDADES { get; set; }
        public virtual DbSet<DATOSDELICTIVOS> DATOSDELICTIVOS { get; set; }
        public virtual DbSet<DATOSPROBLEMADROGAS_CONSUMO> DATOSPROBLEMADROGAS_CONSUMO { get; set; }
        public virtual DbSet<DATOSPROBLEMADROGAS_CONSUMODROGAS> DATOSPROBLEMADROGAS_CONSUMODROGAS { get; set; }
        public virtual DbSet<DATOSPROBLEMADROGAS_DROGAS> DATOSPROBLEMADROGAS_DROGAS { get; set; }
        public virtual DbSet<INFORMACIONACADEMICA_ESCOLARIDAD> INFORMACIONACADEMICA_ESCOLARIDAD { get; set; }
        public virtual DbSet<INFORMACIONACADEMICA_ESTUDIOS> INFORMACIONACADEMICA_ESTUDIOS { get; set; }
        public virtual DbSet<INFORMACIONACADEMICA_ESTUDIOSESCOLARIDAD> INFORMACIONACADEMICA_ESTUDIOSESCOLARIDAD { get; set; }
        public virtual DbSet<INFORMACIONACADEMICA_ESTUDIOSOFICIO> INFORMACIONACADEMICA_ESTUDIOSOFICIO { get; set; }
        public virtual DbSet<INFORMACIONACADEMICA_OFICIOS> INFORMACIONACADEMICA_OFICIOS { get; set; }
        public virtual DbSet<INGRESO> INGRESO { get; set; }
        public virtual DbSet<MOTIVOSINGRESO> MOTIVOSINGRESO { get; set; }
        public virtual DbSet<DATOSPERSONALES1> DATOSPERSONALES1 { get; set; }
        public virtual DbSet<FICHA> FICHA { get; set; }
        public virtual DbSet<DOCUMENTOSINGRESO> DOCUMENTOSINGRESO { get; set; }
        public virtual DbSet<TIPOSDOCUMENTO> TIPOSDOCUMENTO { get; set; }
        public virtual DbSet<CENTRODESARROLLOINGRESO> CENTRODESARROLLOINGRESO { get; set; }
        public virtual DbSet<PSQ_DETALLEENFERMEDADES> PSQ_DETALLEENFERMEDADES { get; set; }
        public virtual DbSet<PSQ_EXAMENPSICOPATOLOGICO> PSQ_EXAMENPSICOPATOLOGICO { get; set; }
        public virtual DbSet<PSQ_HISTORIALINGRESOSPV> PSQ_HISTORIALINGRESOSPV { get; set; }
        public virtual DbSet<PSQ_OBSERVACIONFINAL> PSQ_OBSERVACIONFINAL { get; set; }
        public virtual DbSet<PSQ_SINTOMASPRINCIPALES> PSQ_SINTOMASPRINCIPALES { get; set; }
        public virtual DbSet<PRO_CRITERIO> PRO_CRITERIO { get; set; }
        public virtual DbSet<PRO_FASE> PRO_FASE { get; set; }
        public virtual DbSet<PRO_NIVEL> PRO_NIVEL { get; set; }
        public virtual DbSet<PRO_NIVELCRITERIO> PRO_NIVELCRITERIO { get; set; }
        public virtual DbSet<PRO_PROMOCIONNIVEL> PRO_PROMOCIONNIVEL { get; set; }
        public virtual DbSet<TER_ACTITUD> TER_ACTITUD { get; set; }
        public virtual DbSet<TER_AREACRITERIOCALIF> TER_AREACRITERIOCALIF { get; set; }
        public virtual DbSet<TER_ASPECTOEXAMENPERSONALIDAD> TER_ASPECTOEXAMENPERSONALIDAD { get; set; }
        public virtual DbSet<TER_ASPECTOPERSONALIDAD> TER_ASPECTOPERSONALIDAD { get; set; }
        public virtual DbSet<TER_CALIFAREA> TER_CALIFAREA { get; set; }
        public virtual DbSet<TER_CATEGORIAAREA> TER_CATEGORIAAREA { get; set; }
        public virtual DbSet<TER_CATEGORIAASPECTOCARCTER> TER_CATEGORIAASPECTOCARCTER { get; set; }
        public virtual DbSet<TER_CATEGORIAFALTA> TER_CATEGORIAFALTA { get; set; }
        public virtual DbSet<TER_CATEGORIAPERSONALIDAD> TER_CATEGORIAPERSONALIDAD { get; set; }
        public virtual DbSet<TER_CRITERIOCALIFICACION> TER_CRITERIOCALIFICACION { get; set; }
        public virtual DbSet<TER_CURSO> TER_CURSO { get; set; }
        public virtual DbSet<TER_EVALUACIONCARACTER> TER_EVALUACIONCARACTER { get; set; }
        public virtual DbSet<TER_EXAMENPERSONALIDAD> TER_EXAMENPERSONALIDAD { get; set; }
        public virtual DbSet<TER_FALTA> TER_FALTA { get; set; }
        public virtual DbSet<TER_INCIDENCIAAREA> TER_INCIDENCIAAREA { get; set; }
        public virtual DbSet<TER_INSTRUCTOR> TER_INSTRUCTOR { get; set; }
        public virtual DbSet<TER_MATRICULA> TER_MATRICULA { get; set; }
        public virtual DbSet<TER_RESUMENEVOLUCION> TER_RESUMENEVOLUCION { get; set; }
        public virtual DbSet<PRO_CABANIA> PRO_CABANIA { get; set; }
        public virtual DbSet<PRO_CABANIADESARROLLO> PRO_CABANIADESARROLLO { get; set; }
        public virtual DbSet<CONT_CODOTALONARIO> CONT_CODOTALONARIO { get; set; }
        public virtual DbSet<CONT_ESTADOPAGO> CONT_ESTADOPAGO { get; set; }
        public virtual DbSet<CONT_FACTURA> CONT_FACTURA { get; set; }
        public virtual DbSet<CONT_MES> CONT_MES { get; set; }
        public virtual DbSet<CONT_TALONARIO> CONT_TALONARIO { get; set; }
        public virtual DbSet<CONT_TIPOCONCEPTOPAGO> CONT_TIPOCONCEPTOPAGO { get; set; }
        public virtual DbSet<AWBuildVersion> AWBuildVersion { get; set; }
        public virtual DbSet<DatabaseLog> DatabaseLog { get; set; }
        public virtual DbSet<ErrorLog> ErrorLog { get; set; }
        public virtual DbSet<AppLog> AppLog { get; set; }
        public virtual DbSet<TER_PERMISO> TER_PERMISO { get; set; }
    
        public virtual ObjectResult<Nullable<int>> spCrearProcesamientoIngreso(Nullable<int> iDINGRESO, Nullable<System.DateTime> fECHADIAGNOSTICO)
        {
            var iDINGRESOParameter = iDINGRESO.HasValue ?
                new ObjectParameter("IDINGRESO", iDINGRESO) :
                new ObjectParameter("IDINGRESO", typeof(int));
    
            var fECHADIAGNOSTICOParameter = fECHADIAGNOSTICO.HasValue ?
                new ObjectParameter("FECHADIAGNOSTICO", fECHADIAGNOSTICO) :
                new ObjectParameter("FECHADIAGNOSTICO", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("spCrearProcesamientoIngreso", iDINGRESOParameter, fECHADIAGNOSTICOParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> spEditarRecepcionPaciente(Nullable<int> iDINGRESO, Nullable<System.DateTime> fECHAREALINGRESOPV, string oBSERVACIONES, Nullable<int> iDCABANIA, string uSUARIO)
        {
            var iDINGRESOParameter = iDINGRESO.HasValue ?
                new ObjectParameter("IDINGRESO", iDINGRESO) :
                new ObjectParameter("IDINGRESO", typeof(int));
    
            var fECHAREALINGRESOPVParameter = fECHAREALINGRESOPV.HasValue ?
                new ObjectParameter("FECHAREALINGRESOPV", fECHAREALINGRESOPV) :
                new ObjectParameter("FECHAREALINGRESOPV", typeof(System.DateTime));
    
            var oBSERVACIONESParameter = oBSERVACIONES != null ?
                new ObjectParameter("OBSERVACIONES", oBSERVACIONES) :
                new ObjectParameter("OBSERVACIONES", typeof(string));
    
            var iDCABANIAParameter = iDCABANIA.HasValue ?
                new ObjectParameter("IDCABANIA", iDCABANIA) :
                new ObjectParameter("IDCABANIA", typeof(int));
    
            var uSUARIOParameter = uSUARIO != null ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("spEditarRecepcionPaciente", iDINGRESOParameter, fECHAREALINGRESOPVParameter, oBSERVACIONESParameter, iDCABANIAParameter, uSUARIOParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> spGuardarInfoAcademicaIngreso(Nullable<int> iDINGRESO, Nullable<bool> sABELEERYESCRIBIR, Nullable<int> iDESCOLARIDAD, string dESCRIPCION)
        {
            var iDINGRESOParameter = iDINGRESO.HasValue ?
                new ObjectParameter("IDINGRESO", iDINGRESO) :
                new ObjectParameter("IDINGRESO", typeof(int));
    
            var sABELEERYESCRIBIRParameter = sABELEERYESCRIBIR.HasValue ?
                new ObjectParameter("SABELEERYESCRIBIR", sABELEERYESCRIBIR) :
                new ObjectParameter("SABELEERYESCRIBIR", typeof(bool));
    
            var iDESCOLARIDADParameter = iDESCOLARIDAD.HasValue ?
                new ObjectParameter("IDESCOLARIDAD", iDESCOLARIDAD) :
                new ObjectParameter("IDESCOLARIDAD", typeof(int));
    
            var dESCRIPCIONParameter = dESCRIPCION != null ?
                new ObjectParameter("DESCRIPCION", dESCRIPCION) :
                new ObjectParameter("DESCRIPCION", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("spGuardarInfoAcademicaIngreso", iDINGRESOParameter, sABELEERYESCRIBIRParameter, iDESCOLARIDADParameter, dESCRIPCIONParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> spInsertNewAppLogItem(string tipo, Nullable<System.DateTime> postTime, string componente, string metodo, Nullable<System.TimeSpan> tiempoTomado, string propiedades, string usuario)
        {
            var tipoParameter = tipo != null ?
                new ObjectParameter("Tipo", tipo) :
                new ObjectParameter("Tipo", typeof(string));
    
            var postTimeParameter = postTime.HasValue ?
                new ObjectParameter("PostTime", postTime) :
                new ObjectParameter("PostTime", typeof(System.DateTime));
    
            var componenteParameter = componente != null ?
                new ObjectParameter("Componente", componente) :
                new ObjectParameter("Componente", typeof(string));
    
            var metodoParameter = metodo != null ?
                new ObjectParameter("Metodo", metodo) :
                new ObjectParameter("Metodo", typeof(string));
    
            var tiempoTomadoParameter = tiempoTomado.HasValue ?
                new ObjectParameter("TiempoTomado", tiempoTomado) :
                new ObjectParameter("TiempoTomado", typeof(System.TimeSpan));
    
            var propiedadesParameter = propiedades != null ?
                new ObjectParameter("Propiedades", propiedades) :
                new ObjectParameter("Propiedades", typeof(string));
    
            var usuarioParameter = usuario != null ?
                new ObjectParameter("Usuario", usuario) :
                new ObjectParameter("Usuario", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("spInsertNewAppLogItem", tipoParameter, postTimeParameter, componenteParameter, metodoParameter, tiempoTomadoParameter, propiedadesParameter, usuarioParameter);
        }
    }
}
