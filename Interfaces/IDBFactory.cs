using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SICIApp.Dominio;
using SICIApp.Entities;
using SICIApp.Interfaces;
using SICIApp.Models;
using SICIApp.Services;

namespace SICIApp.Interfaces
{
  public interface IDBFactory
    {
        //IETracker guardado, por múltiples entidades para guardar
        GuardarInfoAcademicaIngreso GuardarInfoAcademica(INFORMACIONACADEMICA_ESTUDIOS _entityBase, INFORMACIONACADEMICA_ESTUDIOSESCOLARIDAD _entity);
    }
}
