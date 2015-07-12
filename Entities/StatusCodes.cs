using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SICIApp.Entities
{
    public enum CrearCentroTerapeutico { 
        Exito = 0,

        CodigoRepetido = 1,

        ErrorGeneral = 2,

        ObjetoNulo = 3,

        ClaveNula = 4,
    }
}