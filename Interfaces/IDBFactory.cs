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
using SICIApp.ViewModels;

namespace SICIApp.Interfaces
{
  public interface IDBFactory
    {
        //IETracker guardado, por múltiples entidades para guardar
        GuardarInfoAcademicaIngreso GuardarInfoAcademica(INFORMACIONACADEMICA_ESTUDIOS _entityBase, INFORMACIONACADEMICA_ESTUDIOSESCOLARIDAD _entity);
        IEnumerable<PRO_CABANIA> GetCabaniasDisponibles(int? IDINGRESO);
        IQueryable<PRO_CABANIA> GetCabaniasDisponibles2(int? IDINGRESO);
        IQueryable<TopDrogasViewModel> GetTopDrogas { get; }

      // proceso de inserción de codos masivos
        GuardarCodosTalonariosMasivos GuardarCodosMasivos(List<CONT_CODOTALONARIO> CodosListToAdd);

    }
}
