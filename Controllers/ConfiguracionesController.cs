using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mail;
using System.Web.Mvc;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SICIApp.Dominio;
using SICIApp.Entities;
using SICIApp.Interfaces;
using SICIApp.Models;
using SICIApp.Services;
using System.Data.Entity.Infrastructure;

namespace SICIApp.Controllers
{
    public class ConfiguracionesController : Controller
    {
        #region Presets
		 //public IDBRepository _repository {get; set;}
         public IEmailSender _emailsender{get; set;}
         public SICIBD2Entities1 _context { get; set; }
         
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            if(this._emailsender == null || _context == null)
            {
           //     this._repository = new DBRepository();
                this._context = new SICIBD2Entities1();
                this._emailsender = new EmailSender();
            }
 	         base.Initialize(requestContext);
        }
	    #endregion

        // GET: Configuraciones
        // 
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }


        #region Acciones para manteniento y configuración de CT
        [Authorize]
        public async Task<ActionResult> CentrosTerapeuticos()
        {
            // se crea la lista de centros Terapeúticos
            // se va a usar el MODEL DE CT
            var _centrosTerapeuticos = _context.CENTROTERAPEUTICOes;
            return View(await _centrosTerapeuticos.ToListAsync());
        }

        // DETALLE DEL CT
        public async Task<ActionResult> DetalleCentroTerapeutico(string ID)
        {
            // si es una cadena vacía, es una petición fallida
            if(ID.Equals(string.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var _centroTerapeutico = await this._context.CENTROTERAPEUTICOes.FindAsync(ID);

            // si el resultado es nulo se envía una mensaje al cliente de no encontrado
            if(_centroTerapeutico == null)
            {
                return HttpNotFound();
            }

            //seteamos el modelo
            var _model = new CENTROTERAPEUTICOMODEL { 
                ID = _centroTerapeutico.ID,
                NOMBRE = _centroTerapeutico.NOMBRE,
                DESCRIPCION = _centroTerapeutico.DESCRIPCION,
                CITY = _context.CITies.Find(_centroTerapeutico.IDCIUDADOPERACION),
                IDCIUDADOPERACION = _centroTerapeutico.IDCIUDADOPERACION

            };

            return View(_model);
        }

        [Authorize]
        public ActionResult NuevoCentroTerapeutico()
        {
            //ViewBag.CityID = new SelectList(_context.CITies, "ID", "NAME");
            var _model = new CENTROTERAPEUTICOMODEL();
            _model.CITIES = _context.CITies;

            return View(_model);
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NuevoCentroTerapeutico(CENTROTERAPEUTICOMODEL _model)
        {
            if (ModelState.IsValid)
            {
                var _newCT = new CENTROTERAPEUTICO
                {
                    ID = _model.ID,
                    NOMBRE = _model.NOMBRE,
                    DESCRIPCION = _model.DESCRIPCION,
                };

                // si existe la clave en el contexto
                if (_context.CENTROTERAPEUTICOes.Find(_model.ID) != null)
                {
                    ModelState.AddModelError(string.Empty, "Esta clave ya ha sido asignada a otro CT");
                    ViewBag.CityID = new SelectList(_context.CITies, "ID", "Ciudad");
                    return View(_model);
                }
                // si el nombre está repetido
                else if (_context.CENTROTERAPEUTICOes.FirstOrDefault(c => c.NOMBRE == _model.NOMBRE) != null)
                {

                    ModelState.AddModelError(string.Empty, "El nombre de este CT ya existe para otro registro");
                    ViewBag.CityID = new SelectList(_context.CITies, "ID", "Ciudad");
                    return View(_model);
                }
                else
                {

                    // se procede a guardar en la base de datos
                    _context.CENTROTERAPEUTICOes.Add(_newCT);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Configuraciones");

                }

                
            }
            // se redirige a la misma página, con el detalle de sus errores
            _model.CITIES = _context.CITies;
            return View(_model);
        }
 
        // Edición de  un CT
        // GET
        [Authorize]
        public async Task<ActionResult> EditarCentroTerapeutico(string ID)
        {
            if(ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var _centroTerapeutico = await _context.CENTROTERAPEUTICOes.FindAsync(ID);

            if(_centroTerapeutico == null)
            {

                return HttpNotFound();

            }

            // model set
            var _model = new CENTROTERAPEUTICOMODEL { 
                ID = _centroTerapeutico.ID,
                NOMBRE = _centroTerapeutico.NOMBRE,
                DESCRIPCION = _centroTerapeutico.DESCRIPCION,
                CITY = _context.CITies.Find(_centroTerapeutico.IDCIUDADOPERACION),
                IDCIUDADOPERACION = _centroTerapeutico.IDCIUDADOPERACION
            };

            _model.CITIES = _context.CITies;
            return View(_model);
        }


        // Edición de  un CT
        // GET

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditarCentroTerapeutico(CENTROTERAPEUTICOMODEL _model)
        {
            try { 
            
                if (ModelState.IsValid)
            {
                var _updateCT = new CENTROTERAPEUTICO
                {
                    ID = _model.ID,
                    NOMBRE = _model.NOMBRE,
                    DESCRIPCION = _model.DESCRIPCION,
                };

                // si existe la clave en el contexto
                
                // si el nombre está repetido
                if (_context.CENTROTERAPEUTICOes.FirstOrDefault(c => c.NOMBRE == _model.NOMBRE) != null)
                {

                    ModelState.AddModelError(string.Empty, "El nombre de este CT ya existe para otro registro");
                    ViewBag.CityID = new SelectList(_context.CITies, "ID", "Ciudad");
                    return View(_model);
                }
                else
                {

                    // se procede a guardar en la base de datos
                    _context.Entry(_updateCT).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return RedirectToAction("CentrosTerapeuticos", "Configuraciones");

                }


            }

            }catch(DbUpdateConcurrencyException ex)
            {
                var vEntry = ex.Entries.Single(); 
                var vClientValue = (CENTROTERAPEUTICO)vEntry.Entity;
                var vDataBaseEntry = vEntry.GetDatabaseValues();

                if(vDataBaseEntry == null)
                {
                    ModelState.AddModelError(string.Empty, "Imposible hacer cambios, el CENTRO TERAPEUTICO ha sido eliminado por otro usuario");
                }else
                {
                    var vDatabaseValues = (CENTROTERAPEUTICO)vDataBaseEntry.ToObject();

                    if(vDatabaseValues.NOMBRE != vClientValue.NOMBRE)
                         ModelState.AddModelError("NOMBRE", "VALOR ACTUAL: " + vDatabaseValues.NOMBRE);
                    if(vDatabaseValues.DESCRIPCION != vClientValue.DESCRIPCION)
                        ModelState.AddModelError("DESCRIPCION", "VALOR ACTUAL: " + String.Format("{0:C}", vDatabaseValues.DESCRIPCION));
                    if(vDatabaseValues.IDCIUDADOPERACION != vClientValue.IDCIUDADOPERACION)
                        ModelState.AddModelError("IDCIUDADOPERACION", "VALOR ACTUAL: "+ _context.CITies.Find(vDatabaseValues.IDCIUDADOPERACION).NAME);
                    ModelState.AddModelError(string.Empty, "El registro que intenta modificar, fue modificado por otro usuario después "
                        +"que se tuvo un valor original. La operación de Edición fue cancelada. Si deseas seguir intentado, guarda e cambio de nuevo, sino "
                        +" haz clic en la lista de todos los registros");

                }
             }

            catch(RetryLimitExceededException /* dex */ )
            {
                // log
                ModelState.AddModelError(string.Empty, "No se pueden guardar los cambios, si el problema persiste, por favor contacar a los administradores del sistema");
            }

            // se redirige a la misma página, con el detalle de sus errores
            _model.CITIES = _context.CITies;
            return View(_model);
        }

        // Json para filtrar dinámicamente las ciudades por su país
        [Authorize]
        public ActionResult SeleccionCiudad(string CODE)
        {
            var vCiudades = (from c in _context.CITies
                             where c.COUNTRYCODE == CODE
                             select new
                             {
                                 c.ID,
                                 c.NAME
                             });

            return Json(vCiudades, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Acciones para mantenimiento y configuración de tipo evaluación médica

        // lista de tipos de evaluación médica
        // lista asíncorona
        // Configuraciones/TiposEvaluacionesMedicas
        public async Task<ActionResult> TiposEvaluacionesMedicas()
        {
            // establcemiento de un intermedio
            var _tiposem = _context.TIPOEVALUACIONMEDICA;
            return View(await _tiposem.ToListAsync());
        }

        // detalle de tipo evaluación médica
        public async Task<ActionResult> DetalleTipoEvaluacionMedica(int? ID)
        {
            // si el ID es nulo o 0
            if(ID == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //buscamos el ID
            //var _S

            return View();
        }

        #endregion


    }
}