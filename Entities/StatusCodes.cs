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

    public enum EnviarEmailCreateStatus
    {
        Exito = 0,

        ErrorDeSMTP = 1,

        ErrorGeneral = 2,

        
    }

    public enum GuardarInfoAcademicaIngreso
    {
        Exito = 0,

        ErorAlGuardarInfoEstudios = 1,

        ErrorAlguardarInfoEstudiosEscolaridad = 2,

        ObjetoNulo = 3,

        ClaveNula = 4,
    }

    public enum GuardarCodosTalonariosMasivos
    {
        Exito = 0,

        ErrorAlGuardarInfo = 1,       

        ErrorGeneral = 2
    }
}