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
using SICIApp.ViewModels;


namespace SICIApp.Dominio
{
    public class DBFactory:IDBFactory
    {
        SICIBD2Entities1 _context = new SICIBD2Entities1();

        public GuardarInfoAcademicaIngreso GuardarInfoAcademica(INFORMACIONACADEMICA_ESTUDIOS _entityBase, INFORMACIONACADEMICA_ESTUDIOSESCOLARIDAD _entity)
        {


            try {

                // actualizar _entitybase ->INFORMACIONACADEMICA_ESTUDIOS
                //agregar un nuevo INFORMACIONACADEMICA_ESTUDIOSESCOLARIDAD _entity
                var res = _context.spGuardarInfoAcademicaIngreso(_entityBase.IDINGRESO, _entityBase.SABELEERYESCRIBIR, _entity.IDESCOLARIDAD, _entity.DESCRIPCION);

                if (res != null)
                {
                    return GuardarInfoAcademicaIngreso.Exito;
                }
                else
                {
                    return GuardarInfoAcademicaIngreso.ErrorAlguardarInfoEstudiosEscolaridad;
                }

            }
            

            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return GuardarInfoAcademicaIngreso.ErorAlGuardarInfoEstudios;
            }
        }

        


        public IEnumerable<PRO_CABANIA> GetCabaniasDisponibles(int? IDINGRESO)
        {
            string query = "DECLARE @IDINGRESO INT "
                            + " SET @IDINGRESO = "+IDINGRESO+""
                            + " SELECT "
	                        +"    CA.*"
                            +" FROM "
	                        +"    [SICI_INGRESOINTERNO].[INGRESO] inn "
	                        +"    INNER JOIN [SICI_INGRESOINTERNO].[CENTRODESARROLLOINGRESO] CDI "
	                        +"    ON inn.ID = CDI.IDINGRESO "	
	                        +"    INNER JOIN "
	                        +"    [SICI_GENERALES].[CENTROTERAPEUTICO] CT "
	                        +"    ON CT.ID = CDI.IDCENTROTERAPEUTICO "	
	                        +"    INNER JOIN 	[SICI_PROMOCION].[PRO_CABANIA] ca "
	                        +"    ON ca.IDCENTROTERAPEUTICO = CT.ID "
                            + " WHERE "
	                        +"    inn.[ID] = @IDINGRESO";
            var data = this._context.Database.SqlQuery<Entities.PRO_CABANIA>(query);

            return data.AsEnumerable();
        }


        public IQueryable<PRO_CABANIA> GetCabaniasDisponibles2(int? IDINGRESO)
        {
            string query = "DECLARE @IDINGRESO INT "
                              + " SET @IDINGRESO = "+IDINGRESO+""
                              + " SELECT "
                              + "    CA.*"
                              + " FROM "
                              + "    [SICI_INGRESOINTERNO].[INGRESO] inn "
                              + "    INNER JOIN [SICI_INGRESOINTERNO].[CENTRODESARROLLOINGRESO] CDI "
                              + "    ON inn.ID = CDI.IDINGRESO "
                              + "    INNER JOIN "
                              + "    [SICI_GENERALES].[CENTROTERAPEUTICO] CT "
                              + "    ON CT.ID = CDI.IDCENTROTERAPEUTICO "
                              + "    INNER JOIN 	[SICI_PROMOCION].[PRO_CABANIA] ca "
                              + "    ON ca.IDCENTROTERAPEUTICO = CT.ID "
                              + " WHERE "
                              + "    inn.[ID] = @IDINGRESO";
            var data = this._context.Database.SqlQuery<Entities.PRO_CABANIA>(query);

            return data.AsQueryable();
        }


        public IQueryable<TopDrogasViewModel> GetTopDrogas
        {
            get {

                string query = "SELECT RANK() OVER( ORDER BY "
                                + "COUNT(DRG.ID),"
                                + "DRG.NOMBRECIENTIFICO DESC) AS RANK, "
		                        +" COUNT(DRG.ID) AS OCURRENCIA,"
		                        +" DRG.NOMBRECIENTIFICO"		                        
                        +" FROM "
	                     +   " [SICI_INGRESOINTERNO].[DATOSPROBLEMADROGAS_CONSUMODROGAS] DG"
	                      +  " INNER JOIN [SICI_INGRESOINTERNO].[DATOSPROBLEMADROGAS_DROGAS] DRG"
	                       + " ON DG.[IDDROGA] = DRG.[ID]"
                        +" WHERE"
	                     +   " DG.IDINGRESO IN ("
						                       +" SELECT [ID]"
						                       + "FROM [SICI_INGRESOINTERNO].[INGRESO]"
	                      +  " )"
	                       + " GROUP BY DRG.[ID], DRG.[NOMBRECIENTIFICO]"
                            + " ORDER BY RANK";
                var data = this._context.Database.SqlQuery<TopDrogasViewModel>(query);

                return data.AsQueryable();
                
            }
        }
    }
}