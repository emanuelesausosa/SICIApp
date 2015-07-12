using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Text;
using SICIApp.Interfaces;

namespace SICIApp.Services
{
    public class Logger: ILogger
    {
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
            string mensaje = String.Concat("Componente:", nombreComponente, ";Método: ", metodo, ";Timespan:",timespan.ToString(), ";Propiedades:",propiedades);
            Trace.TraceInformation(mensaje);
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