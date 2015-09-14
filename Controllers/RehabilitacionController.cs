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
using SICIApp.ViewModels;
using System.Data.Entity.Infrastructure;
using System.Text;

namespace SICIApp.Controllers
{
    public class RehabilitacionController : Controller
    {
        #region Presets
        //public IDBRepository _repository {get; set;}
        public IEmailSender _emailsender { get; set; }
        public SICIBD2Entities1 _context { get; set; }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            if (this._emailsender == null || _context == null)
            {
                //     this._repository = new DBRepository();
                this._context = new SICIBD2Entities1();
                this._emailsender = new EmailSender();
            }
            base.Initialize(requestContext);
        }
        #endregion

        // GET: Rehabilitacion
        // en esta sección se despliega el vistazoi completo de los pacientes
        // por Fases--> Niveles --> Paciente
        // aquí se muestran las fases, niveles y pacientes
        public async Task<ActionResult> Index()
        {
            //var _entity = await _context.INGRESO.Include(i => i.FICHA)
            // .Where(i => i.STATUSFLOW == 1)
            // .Include(i => i.CENTRODESARROLLOINGRESO)
            // .OrderBy(i => i.FECHAINGRESOSISTEMA)
            // .ToListAsync();


            //set entity
            var _viewModels = new RehabilitacionViewModel();

            //set fases
           _viewModels.Fases = await _context.PRO_FASE.ToListAsync();

            //set viewmodel Niveles
            //_viewModels.PromocionesNiveles = await _context.PRO_PROMOCIONNIVEL.Include(i => i.INGRESO)
            //    .Include(i => i.PRO_NIVEL)
            //    .Where(i => i.ESTADO == "ACTIVO").ToListAsync();
            
           
            // set Niveles
            //_viewModels.Niveles = await _context.PRO_NIVEL.ToListAsync();

            return View(_viewModels);
        }

        // procesamiento de solicitudes
        #region Procesamiento de solicitudes
        [Authorize]
        // [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> ProcesarSolicitud(int? IDINGRESO)
        {
            //sim tiempo
            Thread.Sleep(2000);

            // retorno de mensaje de error
            string error = "";

            // validaciones del ingreso
            if (IDINGRESO == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // validar la existencia del ingreso
            // get entity
            var _entity = await _context.INGRESO.FindAsync(IDINGRESO);

            if (_entity == null)
            {
                return HttpNotFound();
            }

            //validar que este ingreso no se haya procesado antes
            // var _pruebaProcesamiento = await _context.DATOSSOCIOECONOMICOS.FindAsync(IDINGRESO);
            //var _pruebaProcesamiento = _entity.STATUSFLOW;

            //if (_pruebaProcesamiento > 1)
            //{
            //    //return RedirectToAction("ResultadoProcesamiento", "Rehabilitacion", new { IDRESUMEN = 1 });
            //    //error al procesar
            //    error = "error al procesar, ya existe un proceso solicitado!";
            //    return Json(new { success = false, message = "error al procesar, ya existe un proceso solicitado!" }, JsonRequestBehavior.AllowGet);
            //}

            try
            {
                //procesamiento
                /* STATUSFLOW
                 * SOL -- 1
                 * MED -- 2
                 * PSQ -- 3
                 * PS  -- 4
                 * TS  -- 5
                 * CONF -- 6
                 * DENG -- 7
                 * ING -- 8
                 * EGRE -- 9
                 * */

                // BUSCAR PROCEDIMIENTO

                _context.spCrearProcesamientoIngreso(IDINGRESO, DateTime.Now);

                return Json(new { success = true, message = "Solicitud Procesada correctamente!" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                error = ex.Message;
                //return RedirectToAction("ResultadoProcesamiento", "Rehabilitacion", new { IDRESUMEN = 2 });
                return Json(new { success = false, message = "No se ha podido procesar la solocitud" }, JsonRequestBehavior.AllowGet);
            }


        }

        // get
        public async Task<ActionResult> ProcesarSolicitudAlterna(int? ID)
        {
            try
            {

                // validaciones del ingreso
                if (ID == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                // validar la existencia del ingreso
                // get entity
                var _entity = await _context.INGRESO.FindAsync(ID);


                if (_entity == null)
                {
                    return HttpNotFound();
                }

                var _model = new INGRESOMODEL
                {
                    ID = _entity.ID,
                    STATUSFLOW = _entity.STATUSFLOW,
                    FICHA = _entity.FICHA,
                    OBSERVACIONES = _entity.OBSERVACIONES,
                    FECHAINGRESOSISTEMA = _entity.FECHAINGRESOSISTEMA


                };

                // luego se comprueba algunos detalles
                //si ese ingreso está activo
                //solicitud de ficha por ingreso
                //_entity.FICHA = await _context.FICHA.FindAsync(_entity.IDPERSONA);

                //
                _model.actionName = actionNameFunc((int)_model.STATUSFLOW);
                //return View(_entity);
                return PartialView("_ProcesarSolicitudAlterna", _model);

            }
            catch (Exception ex)
            {
                return RedirectToAction("DetalleErrorProcesamiento", "Rehabilitacion", new { detalle = ex.Message });
            }
        }

        private string actionNameFunc(int flowId)
        {
            //procesamiento
            /* STATUSFLOW
             * SOL -- 1
             * MED -- 2
             * PSQ -- 3
             * PS  -- 4
             * TS  -- 5
             * CONF -- 6
             * DENG -- 7
             * ING -- 8
             * EGRE -- 9
             * */

            string actionName = "";

            switch (flowId)
            {
                case 1: actionName = "Solicitudes";
                    return actionName;
                case 2: actionName = "Medicina";
                    return actionName;
                case 3: actionName = "Psiquiatria";
                    return actionName;
                case 4: actionName = "Psicologia";
                    return actionName;
                case 5: actionName = "TrabajoSocial";
                    return actionName;
                case 6: actionName = "SolicitudesPendientesAprobar";
                    return actionName;
                case 7: actionName = "HistoricosNoAceptadas";
                    return actionName;
                case 8: actionName = "NuevosIngresos";
                    return actionName;
                default: return actionName = "Solicitudes";
            }
        }

        // post de procesamieto alterno 
        //
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ProcesarSolicitudAlterna(INGRESOMODEL _model)
        {
            //sim tiempo
            Thread.Sleep(2000);

            // retorno de mensaje de error
            string error = "";

            // validaciones del ingreso
            if (_model.ID == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // validar la existencia del ingreso
            // get entity
            var _entity = await _context.INGRESO.FindAsync(_model.ID);

            if (_entity == null)
            {
                return HttpNotFound();
            }

            //validar que este ingreso no se haya procesado antes
            // var _pruebaProcesamiento = await _context.DATOSSOCIOECONOMICOS.FindAsync(IDINGRESO);
            //var _pruebaProcesamiento = _entity.STATUSFLOW;

            //if (_pruebaProcesamiento > 1)
            //{
            //    //return RedirectToAction("ResultadoProcesamiento", "Rehabilitacion", new { IDRESUMEN = 1 });
            //    //error al procesar
            //    error = "error al procesar, ya existe un proceso solicitado!";
            //    return Json(new { success = false, message = "error al procesar, ya existe un proceso solicitado!" }, JsonRequestBehavior.AllowGet);
            //}

            try
            {
                //procesamiento
                /* STATUSFLOW
                 * SOL -- 1
                 * MED -- 2
                 * PSQ -- 3
                 * PS  -- 4
                 * TS  -- 5
                 * CONF -- 6
                 * DENG -- 7
                 * ING -- 8
                 * EGRE -- 9
                 * */

                // BUSCAR PROCEDIMIENTO

                _context.spCrearProcesamientoIngreso(_model.ID, DateTime.Now);

                //return Json(new { success = true, message = "Solicitud Procesada correctamente!" }, JsonRequestBehavior.AllowGet);
                return RedirectToAction(_model.actionName, "Rehabilitacion");

            }
            catch (Exception ex)
            {
                error = ex.Message;
                //return RedirectToAction("ResultadoProcesamiento", "Rehabilitacion", new { IDRESUMEN = 2 });
                // return Json(new { success = false, message = "No se ha podido procesar la solocitud" }, JsonRequestBehavior.AllowGet);
                return RedirectToAction("DetalleErrorProcesamiento", "Rehabilitacion", new { detalle = ex.Message });
            }
        }

        #endregion

        // procesamineto de nuevos ingresos
        // flow 8, para los que sí fueron aceptados es el 8
        #region Procesamiento para nuevos pacientes 
        public async Task<ActionResult> NuevosIngresos()
        {
            // obtener las solicitudes de ingreso
            // primero los ingresos
            //get entity
            // se filtran las que están en proceso SOLICITADO, STATUSFLOW = 1
            var _entity = await _context.INGRESO.Include(i => i.FICHA)
                .Where(i => i.STATUSFLOW == 8)
                .Include(i => i.CENTRODESARROLLOINGRESO)
                .OrderBy(i => i.FECHAINGRESOSISTEMA)
                .ToListAsync();

            // set model
            List<INGRESOMODEL> _model = new List<INGRESOMODEL>();


            //set model ingresos
            foreach (INGRESO entity in _entity)
            {
                _model.Add(
                    new INGRESOMODEL
                    {
                        ID = entity.ID,
                        NUMEXPEDIENTE = entity.NUMEXPEDIENTE,
                        FECHAUTORIZACION = entity.FECHAUTORIZACION,
                        FECHAINGRESOSISTEMA = entity.FECHAINGRESOSISTEMA,
                        FECHAFIRMAACUERDO = entity.FECHAFIRMAACUERDO,
                        FECHAREALINGRESOPV = entity.FECHAREALINGRESOPV,
                        FECHAEGRESOPV = entity.FECHAEGRESOPV,
                        OBSERVACIONES = entity.OBSERVACIONES,
                        CONTRATO = entity.CONTRATO,
                        IDPERSONA = entity.IDPERSONA,
                        ACEPTADO = entity.ACEPTADO,
                        STATUSFLOW = entity.STATUSFLOW,
                        CENTRODESARROLLOINGRESO = entity.CENTRODESARROLLOINGRESO,
                        FICHA = entity.FICHA
                    });

            }

            // to view
            return View(_model);
        }

        // procesamieto de recepción de persona y documentos
        // esto es dentro del centro terapéutico
        public async Task<ActionResult> RecepcionPaciente(int? ID)
        {
            // validaciones del ingreso
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // validar la existencia del ingreso
            // get entity
            var _entity = await _context.INGRESO.FindAsync(ID);


            if (_entity == null)
            {
                return HttpNotFound();
            }

            // luego se comprueba algunos detalles
            //si ese ingreso está activo
            //solicitud de ficha por ingreso
            _entity.FICHA = await _context.FICHA.FindAsync(_entity.IDPERSONA);

            // set IOC para obtener las canañas por centro
            IDBFactory _factory = new DBFactory();

            //elementos dinámicos
           ViewBag.IDCABANIA = new SelectList(_factory.GetCabaniasDisponibles(_entity.ID), "ID", "NOMBRE");
            
            // TO VIEW
            return View(_entity);
        }

        public ActionResult GetCabanias(int? IDINGRESO)
        {
            var cabanias = from c in _context.PRO_CABANIA
                           where c.CENTROTERAPEUTICO.Equals(_context.CENTRODESARROLLOINGRESO.FirstOrDefault(i => i.IDINGRESO == IDINGRESO && i.ACTIVO == true).IDCENTROTERAPEUTICO)
                           select new { 
                               c.ID,
                               c.NOMBRE
                           };
            return Json(cabanias, JsonRequestBehavior.AllowGet);
        }

        // validación  de la información de recepción
        //procesamiento
        public async Task<ActionResult> EditarRecepcionPaciente(INGRESOMODEL _model, int? IDCABANIA)
        {
            Thread.Sleep(1000);

            try
            {

                // get entity 
                var _entity = new INGRESO();    

                // find
                _entity = await _context.INGRESO.FindAsync(_model.ID);


                // guardar mediante la fucnión spEditar

                var _res = _context.spEditarRecepcionPaciente(_entity.ID, _model.FECHAREALINGRESOPV, _model.OBSERVACIONES, IDCABANIA, User.Identity.Name);

                if (_res != null)
                {
                    return Json(new { success = true, message = "Éxito al guardar información" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "No se ha guardado el registro :(" }, JsonRequestBehavior.AllowGet);
                }
                ////set cambios                
                //_entity.FECHAREALINGRESOPV = _model.FECHAREALINGRESOPV;
                //_entity.OBSERVACIONES = _model.OBSERVACIONES;

                ////save data 
                //_context.Entry(_entity).State = EntityState.Modified;
                //await _context.SaveChangesAsync();

                

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        } 
        #endregion

        // perfil del paciente
        #region Prefil del paciente
        //get
        public async Task<ActionResult> PerfilPaciente(int? IDFICHA)
        {
            // comprobar que el id no es nulo
            if(IDFICHA == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            //get entity
            var _entity = await _context.FICHA.FindAsync(IDFICHA);

            //comprobar que existe la entitad
            if(_entity == null)
            {
                return HttpNotFound();
            }
                       

            //to view
            return View(_entity);
        }
        #endregion
        
    }
}