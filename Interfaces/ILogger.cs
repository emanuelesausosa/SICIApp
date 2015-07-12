using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICIApp.Interfaces
{
    public interface ILogger
    {
        // métodos de impresión de información general
        void Informacion(string mensaje);
        void Informacion(string fmt, params object[] vars);
        void Informacion(Exception excepcion, string fmt, params object[] vars);

        // métodos de empresión de advertencia
        void Advertencia(string mensaje);
        void Advertencia(string fmt, params object[] vars);
        void Advertencia(Exception excepcion, string fmt, params object[] vars);

        // métodos de impresión de Errores
        void Error(string mensaje);
        void Error(string fmt, params object[] vars);
        void Error(Exception excepcion, string fmt, params object[] vars);

        // secuencias de trazas
        void TraceApi(string nombreComponente, string metodo, TimeSpan timespan);
        void TraceApi(string nombreComponente, string metodo, TimeSpan timespan, string propiedades);
        void TraceApi(string nombreComponente, string metodo, TimeSpan timespan, string fmt, params object[] vars);
    }
}
