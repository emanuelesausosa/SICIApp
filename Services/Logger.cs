using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Text;
using System.Net;
using System.Web.Helpers;
using System.Web.Mail;
using System.Web.Mvc;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SICIApp.Dominio;
using SICIApp.Entities;
using SICIApp.Models;
using SICIApp.Services;
using SICIApp.Interfaces;
using System.Data.Entity.Infrastructure;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SICIApp.Services
{
    public class Logger: ILogger
    {
        #region Presets
        //public IDBRepository _repository {get; set;}
        public IEmailSender _emailsender { get; set; }
        public SICIBD2Entities1 _context { get; set; }

        //protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        //{
        //    if (this._emailsender == null || _context == null)
        //    {
        //        //     this._repository = new DBRepository();
        //        this._context = new SICIBD2Entities1();
        //        this._emailsender = new EmailSender();
        //    }           
        //}
        #endregion

        // métodos de impresión de información general
        public void Informacion(string mensaje)
        {
            Trace.TraceInformation(mensaje);
        }

        public void Informacion(string fmt, params object[] vars)
        {
            Trace.TraceInformation(fmt, vars);
        }

        public void Informacion(Exception excepcion, string fmt, params object[] vars)
        {
            Trace.TraceInformation(FormatExceptionMessage(excepcion, fmt, vars));
        }

        // métodos de empresión de advertencia
        public void Advertencia(string mensaje)
        {
            Trace.TraceWarning(mensaje);
        }

        public void Advertencia(string fmt, params object[] vars)
        {
            Trace.TraceWarning(fmt, vars);
        }

        public void Advertencia(Exception excepcion, string fmt, params object[] vars)
        {
            Trace.TraceWarning(FormatExceptionMessage(excepcion, fmt, vars));
        }

        // métodos de impresión de Errores
        public void Error(string mensaje)
        {
            Trace.TraceError(mensaje);
        }

        public void Error(string fmt, params object[] vars)
        {
            Trace.TraceError(fmt, vars);
        }

        public void Error(Exception excepcion, string fmt, params object[] vars)
        {
            Trace.TraceError(FormatExceptionMessage(excepcion, fmt, vars));
        }

        // secuencias de trazas
        public void TraceApi(string nombreComponente, string metodo, TimeSpan timespan)
        {
            TraceApi(nombreComponente, metodo, timespan, "");
        }

        public void TraceApi(string nombreComponente, string metodo, TimeSpan timespan, string propiedades)
        {
            

            IdentityUser User = new IdentityUser();

            string mensaje = String.Concat("Componente:", nombreComponente, ";Método: ", metodo, ";Timespan:",timespan.ToString(), ";Propiedades:",propiedades);

            //var appLog =  new AppLog
            //{
            //    Tipo = nombreComponente,
            //    PostTime = DateTime.Now,
            //    Componente = nombreComponente,
            //    Metodo = metodo,
            //    TiempoTomado = timespan,
            //    Propiedades = propiedades,
            //    Usuario = User.Email
            //};

            Trace.TraceInformation(mensaje);

            //AddToContext(appLog);
            
        }

        private void AddToContext(AppLog appLog)
        {
            _context = new SICIBD2Entities1();
            _context.spInsertNewAppLogItem(appLog.Tipo, appLog.PostTime, appLog.Componente, appLog.Metodo, appLog.TiempoTomado, appLog.Propiedades, appLog.Usuario);
        }

        public void TraceApi(string nombreComponente, string metodo, TimeSpan timespan, string fmt, params object[] vars)
        {
            TraceApi(nombreComponente, metodo, timespan, string.Format(fmt, vars));
        }

        // método que concatena y formatea los mensajes de Error
        private string FormatExceptionMessage(Exception excepcion, string fmt, object[] vars)
        {
            var sb = new StringBuilder();
            sb.Append(string.Format(fmt, vars));
            sb.Append(" Excepción: ");
            sb.Append(excepcion.ToString());
            return sb.ToString();
        }
    }
}