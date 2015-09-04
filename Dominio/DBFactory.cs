using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SICIApp.Dominio;
using SICIApp.Entities;
using SICIApp.Interfaces;
using SICIApp.Models;
using SICIApp.Services;
using System.Data.Entity.Validation;


namespace SICIApp.Dominio
{
    public class DBFactory:IDBFactory
    {
        SICIBD2Entities1 _context = new SICIBD2Entities1();

        public GuardarInfoAcademicaIngreso GuardarInfoAcademica(INFORMACIONACADEMICA_ESTUDIOS _entityBase, INFORMACIONACADEMICA_ESTUDIOSESCOLARIDAD _entity)
        {
            _context = null;
            _context = new SICIBD2Entities1();
           // _context.Dispose();

            try { 

                // obtener el 
                //var entityBase = new INFORMACIONACADEMICA_ESTUDIOS();
                //entityBase = _entityBase;

                //guardar _entity --> navegar hasta el deep
              // int res = GuardarInfoAcademicaEstudiosEscolariad(_entity);
                
                //if(res != 0)
                //{
                    //_context.Entry(_entityBase).State = System.Data.Entity.EntityState.Modified;
                    _context.ChangeTracker.DetectChanges();
                    _context.Entry(_entityBase).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                    return GuardarInfoAcademicaIngreso.Exito;
                //}

              //  return GuardarInfoAcademicaIngreso.ErrorAlguardarInfoEstudiosEscolaridad;

            }
            //catch (DbEntityValidationException dbex)
            //{
            //    foreach (var mdlErrors in dbex.EntityValidationErrors)
            //    {
            //        foreach (var err in mdlErrors.ValidationErrors)
            //        {
            //            System.Console.WriteLine("Property:{0} Error:{1}", err.PropertyName, err.ErrorMessage);
            //        }
            //    }

            //    return GuardarInfoAcademicaIngreso.ObjetoNulo;
            //}

            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return GuardarInfoAcademicaIngreso.ErorAlGuardarInfoEstudios;
            }
        }

        private int GuardarInfoAcademicaEstudiosEscolariad(INFORMACIONACADEMICA_ESTUDIOSESCOLARIDAD _entity)
        {
            try {
                _context.INFORMACIONACADEMICA_ESTUDIOSESCOLARIDAD.Add(_entity);
                _context.SaveChanges();

                return 1;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return 0;
            }
        }
    }
}