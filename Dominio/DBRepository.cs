using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Validation;
using SICIApp.Entities;
using SICIApp.Interfaces;


namespace SICIApp.Dominio
{
    public class DBRepository : IDBRepository
    {
        private SICIBD2Entities1 _DBContext = new SICIBD2Entities1();


        #region Mantenimientos Centros Terapéuticos
        async System.Threading.Tasks.Task<CrearCentroTerapeutico> IDBRepository.CrearCentroTerapeutico(CENTROTERAPEUTICO _CentroTerapeutico)
        {
            // el objeto no puede ser nulo
            if (_CentroTerapeutico.Equals(null))
            {
                return Entities.CrearCentroTerapeutico.ObjetoNulo;
            }
            else
                //el id del objeto entrante no debe ser nullo
                if (_CentroTerapeutico.ID.Equals(string.Empty))
                {

                    return Entities.CrearCentroTerapeutico.ClaveNula;
                }
                else
                    //no debe existir un registro con el mismo ID
                    if (_DBContext.CENTROTERAPEUTICOes.Find(_CentroTerapeutico.ID) != null)
                    {
                        return Entities.CrearCentroTerapeutico.CodigoRepetido;
                    }
                    else
                    {
                        try
                        {

                            //se procede a insertar el registro
                            _DBContext.CENTROTERAPEUTICOes.Add(_CentroTerapeutico);
                            await _DBContext.SaveChangesAsync();
                            return Entities.CrearCentroTerapeutico.Exito;

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.StackTrace);
                            return Entities.CrearCentroTerapeutico.ErrorGeneral;
                        }
                    }
        } 
        #endregion
    }
}