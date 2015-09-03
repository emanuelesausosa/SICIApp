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
using System.Text;

namespace SICIApp.Controllers
{
    public class SolicitudIngresoController : Controller
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

        // GET: SolicitudIngreso
        public ActionResult Index()
        {
            return View();
        }

        // sección de solicidudes de ingreso        
        //Acciones de Solicitudes -- flow 1
        #region Acciones de Solicitudes, Flow 1 - primer paso
        public async Task<ActionResult> Solicitudes()
        {
            // obtener las solicitudes de ingreso
            // primero los ingresos
            //get entity
            // se filtran las que están en proceso SOLICITADO, STATUSFLOW = 1
            var _entity = await _context.INGRESO.Include(i => i.FICHA)
                .Where(i=>i.STATUSFLOW == 1)
                .Include(i=>i.CENTRODESARROLLOINGRESO)
                .OrderBy(i=>i.FECHAINGRESOSISTEMA)
                .ToListAsync();

            // set model
            List<INGRESOMODEL> _model = new List<INGRESOMODEL>();
           
                        
            //set model ingresos
            foreach(INGRESO entity in _entity)
            {
                _model.Add(
                    new INGRESOMODEL { 
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

        
        // Nueva Solicitud
        public ActionResult NuevaSolicitud()
        {
            return View();
        }

        // Proceso de solicitud de nuevo ingreso
        //1. nueva ficha
        //2. Datos personales de la ficha
        //3. Ingreso
        //4. Documentos a presentar      


        //1. Nueva Ficha
        // en esta acción, se ingresa la ficha del paciente a ingresar 
        // se toman los tados básicos 
        // luego se re-direcciona a Datos personales
        // una vista por acción

        //Nueva Ficha
        //GET
        public ActionResult NuevaFicha()
        {
            // get model 
            var _model = new FICHAMODEL();
            return View(_model);
        }

        // POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NuevaFicha(FICHAMODEL _model)
        {
            StringBuilder build = new StringBuilder();
            build.Append(_model.NUMEROIDENTIDAD.ToString());
            build.Remove(4, 1);
            build.Remove(8, 1);

            _model.NUMEROIDENTIDAD = build.ToString();
            // refs urls
            //var vUrlFichas = 
            if (ModelState.IsValid)
            {
                // comprobar en el model de la identidad o el pasaporte               
                if (_model.NUMEROPASAPORTE == null || _model.NUMEROIDENTIDAD == null)
                {
                    ModelState.AddModelError("", "Debe de ingresar almenos el número de identidad o el número de pasaporte");
                    return View(_model);
                }
                //comprobar la existencia de ese número de indentidad
                if (_model.NUMEROIDENTIDAD != null)
                {
                    // ya xiste una persona con este número?
                    if (await _context.FICHA.FirstOrDefaultAsync(f => f.NUMEROIDENTIDAD == _model.NUMEROIDENTIDAD) != null)
                    {
                        var vFicha = await _context.FICHA.FirstOrDefaultAsync(f => f.NUMEROIDENTIDAD == _model.NUMEROIDENTIDAD);

                        ModelState.AddModelError("", "ya existe una Ficha con este número de idientidad, por favor diríjase a: " + Url.Action("DetalleFicha", "SolicitudIngreso", new { ID = vFicha.ID }) + " para una referencia de esta Ficha");
                        return View(_model);
                    }
                    else
                    {
                        // se guarda el nuevo registro 
                        // get y set entity
                        var _entity = new FICHA
                        {
                            NUMEROIDENTIDAD = _model.NUMEROIDENTIDAD,
                            NUMEROPASAPORTE = _model.NUMEROPASAPORTE,
                            PRIMERNOMBRE = _model.PRIMERNOMBRE,
                            SEGUNDONOMBRE = _model.SEGUNDONOMBRE,
                            PRIMERAPELLIDO = _model.PRIMERAPELLIDO,
                            SEGUNDOAPELLIDO = _model.SEGUNDOAPELLIDO,
                            NACIONALIDAD = _model.NACIONALIDAD
                        };

                        // guardar los cambios
                        _context.FICHA.Add(_entity);
                        await _context.SaveChangesAsync();

                        // redirecconar a Datos personales
                        return RedirectToAction("DatosPersonalesFicha", "SolicitudIngreso", new { ID = _entity.ID });
                    }

                }
                else
                    if (_model.NUMEROPASAPORTE != null)
                    {
                        // ya xiste una persona con este número?
                        if (await _context.FICHA.FirstOrDefaultAsync(f => f.NUMEROIDENTIDAD == _model.NUMEROIDENTIDAD) != null)
                        {
                            var vFicha = await _context.FICHA.FirstOrDefaultAsync(f => f.NUMEROIDENTIDAD == _model.NUMEROIDENTIDAD);

                            ModelState.AddModelError("", "ya existe una Ficha con este número de idientidad, por favor diríjase a: " + Url.Action("DetalleFicha", "SolicitudIngreso", new { ID = vFicha.ID }) + " para una referencia de esta Ficha");
                            return View(_model);
                        }
                        else
                        {
                            // se guarda el nuevo registro 
                            // get y set entity
                            var _entity = new FICHA
                            {
                                NUMEROIDENTIDAD = _model.NUMEROIDENTIDAD,
                                NUMEROPASAPORTE = _model.NUMEROPASAPORTE,
                                PRIMERNOMBRE = _model.PRIMERNOMBRE,
                                SEGUNDONOMBRE = _model.SEGUNDONOMBRE,
                                PRIMERAPELLIDO = _model.PRIMERAPELLIDO,
                                SEGUNDOAPELLIDO = _model.SEGUNDOAPELLIDO,
                                NACIONALIDAD = _model.NACIONALIDAD
                            };

                            // guardar los cambios
                            _context.FICHA.Add(_entity);
                            await _context.SaveChangesAsync();

                            // crea una nueva carpeta en 

                            // redirecconar a Datos personales
                            return RedirectToAction("DatosPersonalesFicha", "SolicitudIngreso", new { ID = _entity.ID });
                        }
                    }

            }
            return View(_model);
        }

        //2. Datos personales
        // en esta sección se piden datos complementarios sobre la infomación 
        // básica del aspirante a rehabilitación 
        // se recibe el id de la nueva ficha

        //Datos Personales
        // GET
        public async Task<ActionResult> DatosPersonalesFicha(int? ID)
        {
            // COMPROBar que no es nula
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //get entiy
            var _entity = await _context.FICHA.FindAsync(ID);

            //si es nula, retorna no encontrada
            if (_entity == null)
            {
                return HttpNotFound();
            }

            // set model
            var _model = new DATOSPERSONALESFICHAMODEL();

            _model.FICHA = new FICHA
            {
                ID = _entity.ID,
                NUMEROIDENTIDAD = _entity.NUMEROIDENTIDAD,
                NUMEROPASAPORTE = _entity.NUMEROPASAPORTE,
                PRIMERNOMBRE = _entity.PRIMERNOMBRE,
                SEGUNDONOMBRE = _entity.SEGUNDONOMBRE,
                PRIMERAPELLIDO = _entity.PRIMERAPELLIDO,
                SEGUNDOAPELLIDO = _entity.SEGUNDOAPELLIDO,
                NACIONALIDAD = _entity.NACIONALIDAD
            };

            // set id
            _model.IDPERSONA = _entity.ID;
            // set países
            _model.PAISES = _context.COUNTRies;
            // view bags
            ViewBag.ESTADOCIVIL = new SelectList(EstadosCiviles, "Value", "Text");
            ViewBag.IDPAISNACIONALIDAD = new SelectList(_context.COUNTRies, "CODE", "NAME");

            // to view
            return View(_model);
        }
        
        // estados civiles
        public static List<SelectListItem> EstadosCiviles  = new List<SelectListItem>
            {
                new SelectListItem{Value="SOLTERO", Text="SOLTERO"},
                new SelectListItem{Value="CASADO", Text="CASADO"}
            };

        // datos personales ficha
        //POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DatosPersonalesFicha(DATOSPERSONALESFICHAMODEL _model)
        {
            if (ModelState.IsValid)
            {
                // se guarda el registro
                // get entity
                var _entity = new DATOSPERSONALES1
                {
                    IDPERSONA = _model.FICHA.ID,
                    DIRECCIONDOMICILIO = _model.DIRECCIONDOMICILIO,
                    TELEFONOFIJO = _model.TELEFONOFIJO,
                    TELEFONOMOVIL = _model.TELEFONOMOVIL,
                    FECHANACIMIENTO = _model.FECHANACIMIENTO,
                    ESTADOCIVIL = _model.ESTADOCIVIL,
                    NUMERODEHIJOS = _model.NUMERODEHIJOS,
                    NOMBREMADRE = _model.NOMBREMADRE,
                    VIVECONMADRE = _model.VIVECONMADRE,
                    NOMBREPADRE = _model.NOMBREPADRE,
                    VIVECONPADRE = _model.VIVECONPADRE,
                    CONQUIENVIVE = _model.CONQUIENVIVE,
                    NUMEROHERMANOS = _model.NUMEROHERMANOS,
                    IDPAISNACIONALIDAD = _model.IDPAISNACIONALIDAD,
                    IDCIUDADNATAL = _model.IDCIUDADNATAL,
                    IDCIUDADRESIDENCIAACTUAL = _model.IDCIUDADRESIDENCIAACTUAL
                };

                // guardar el registro
                _context.DATOSPERSONALES1.Add(_entity);
                await _context.SaveChangesAsync();

                // crear carpeta de documentos
                var pathFichaDocus = Path.Combine(Server.MapPath("~/App_Data/DocumentosIngreso"), _entity.IDPERSONA.ToString());

                //orden de crear carpeta
                Directory.CreateDirectory(pathFichaDocus);

                // redireccionar a Ingreso
                return RedirectToAction("InfoNuevoIngreso", "SolicitudIngreso", new { ID = _entity.IDPERSONA });
            }
            // view bags
            _model.PAISES = _context.COUNTRies;
            ViewBag.ESTADOCIVIL = new SelectList(EstadosCiviles, "Value", "Text");
            ViewBag.IDPAISNACIONALIDAD = new SelectList(_context.COUNTRies, "CODE", "NAME", _model.IDPAISNACIONALIDAD);
            return View(_model);
        }

        //3. Info Nuevo ingreso
        // un nuevo ingreso está relacionado a una ficha única
        //
        //GET
        public async Task<ActionResult> InfoNuevoIngreso(int? ID)
        {
            // COPROBAR QUE LA entrada es válida
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //get entiy
            var _entity = await _context.FICHA.FindAsync(ID);

            //si es nula, retorna no encontrada
            if (_entity == null)
            {
                return HttpNotFound();
            }

            // set model
            var _model = new INGRESOMODEL();
            _model.FICHA = _entity;

            _model.IDPERSONA = _entity.ID;

            //Viewbags

            //to view
            ViewBag.IDCRENTROTERAPEUTICO = new SelectList(_context.CENTROTERAPEUTICOes, "ID", "NOMBRE");
            return View(_model);
        }

        // INFO NUEVO INGRESO
        //POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> InfoNuevoIngreso(INGRESOMODEL _model)
        {
            if (ModelState.IsValid)
            {
                // validar si no existe un egreso pendiente
                //tiene al menos un ingreso registrado (re-ingreso)
                if (await _context.INGRESO.FirstOrDefaultAsync(i => i.IDPERSONA == _model.IDPERSONA) != null)
                {
                    // comprueba que al menos un registro tenga fecha de egrso nula
                    if (await _context.INGRESO.FirstOrDefaultAsync(i => i.FECHAEGRESOPV != null) != null)
                    {
                        // duvuele un mesaje de egreso pendiente, no puede tener un egreso pendiente
                        ModelState.AddModelError("", "Este paciente tien un Egreso pendiente, por favor notificar al Director sobre este caso");
                        ViewBag.IDCRENTROTERAPEUTICO = new SelectList(_context.CENTROTERAPEUTICOes, "ID", "NOMBRE", _model.IDCRENTROTERAPEUTICO);
                        return View(_model);
                    }
                    else
                    {
                        // get y set entity
                        var _entity = new INGRESO
                        {
                            NUMEXPEDIENTE = _model.NUMEXPEDIENTE,
                            FECHAINGRESOSISTEMA = DateTime.Now,
                            OBSERVACIONES = _model.OBSERVACIONES,
                            STATUSFLOW = 1,
                            IDPERSONA = _model.IDPERSONA
                        };

                        // guardar 
                        _context.INGRESO.Add(_entity);
                        await _context.SaveChangesAsync();

                        // crear carpeta de documentos
                        var pathFichaDocus = Path.Combine(Server.MapPath("~/App_Data/DocumentosIngreso/" + _entity.IDPERSONA.ToString() + ""), _entity.ID.ToString());

                        //orden de crear carpeta
                        Directory.CreateDirectory(pathFichaDocus);

                        // establecer el centroterapéutico de desarrollo
                        var _entityCTD = new CENTRODESARROLLOINGRESO
                        {
                            
                            IDINGRESO = _entity.ID,
                            IDCENTROTERAPEUTICO = _model.IDCRENTROTERAPEUTICO,
                            FECHAREGISTRO = DateTime.Now,
                            
                        };

                        // guardar el registro
                        _context.CENTRODESARROLLOINGRESO.Add(_entityCTD);
                        await _context.SaveChangesAsync();

                        // redireccionar a los documentos
                        // los docuemtos son dependencia del ingreso
                        return RedirectToAction("EntregaDocumentosIngreso", "SolicitudIngreso", new { ID = _entity.ID });
                    }
                }
                else
                {
                    // get y set entity
                    var _entity = new INGRESO
                    {
                        NUMEXPEDIENTE = _model.NUMEXPEDIENTE,
                        FECHAINGRESOSISTEMA = DateTime.Now,
                        OBSERVACIONES = _model.OBSERVACIONES,
                        STATUSFLOW = 1,
                        IDPERSONA = _model.IDPERSONA
                    };

                    // guardar 
                    _context.INGRESO.Add(_entity);
                    await _context.SaveChangesAsync();

                    // establecer el centroterapéutico de desarrollo
                    var _entityCTD = new CENTRODESARROLLOINGRESO
                    {

                        IDINGRESO = _entity.ID,
                        IDCENTROTERAPEUTICO = _model.IDCRENTROTERAPEUTICO,
                        FECHAREGISTRO = DateTime.Now,

                    };

                    // guardar el registro
                    _context.CENTRODESARROLLOINGRESO.Add(_entityCTD);
                    await _context.SaveChangesAsync();

                    // crear carpeta de documentos por ingreso
                    // crear capeta de interno
                    // crear carpeta de documentos
                    var pathFichaDocus = Path.Combine(Server.MapPath("~/App_Data/DocumentosIngreso/" + _entity.IDPERSONA.ToString() + ""), _entity.ID.ToString());

                    //orden de crear carpeta
                    Directory.CreateDirectory(pathFichaDocus);
                    // redireccionar a los documentos
                    // los docuemtos son dependencia del ingreso
                    return RedirectToAction("EntregaDocumentosIngreso", "SolicitudIngreso", new { ID = _entity.ID });
                }

            }

            ViewBag.IDCRENTROTERAPEUTICO = new SelectList(_context.CENTROTERAPEUTICOes, "ID", "NOMBRE", _model.IDCRENTROTERAPEUTICO);
            return View(_model);
        }

        //3. documentos a entregar
        // los documentos a  entregar por parte del paciente
        // consta de un GET con el modelo y un AJAX get que guarda y actualiza 
        //GET - 
        public async Task<ActionResult> EntregaDocumentosIngreso(int? ID)
        {
            // COPROBAR QUE LA entrada es válida
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //get entiy
            var _entity = await _context.INGRESO.FindAsync(ID);

            //si es nula, retorna no encontrada
            if (_entity == null)
            {
                return HttpNotFound();
            }

            // Set model
            var _modelView = new DOCUMENTOSINGRESOVIEWMODEL();
            _modelView.INGRESO = _entity;

             // set tipos de documentos
            var _tiposdocus = await _context.TIPOSDOCUMENTO.ToListAsync();

            _modelView.TIPOSDOCUMENTOSINGRESOVIEWM = new List<TIPOSDOCUMENTO>();

            foreach(TIPOSDOCUMENTO docs in _tiposdocus)
            {
                _modelView.TIPOSDOCUMENTOSINGRESOVIEWM.ToList().Add(new TIPOSDOCUMENTO { 
                    ID = docs.ID,
                    NOMBRETIPO = docs.NOMBRETIPO,
                    DESCRIPCION = docs.DESCRIPCION
                });
            }

            //to view
            return View(_modelView);
        }

        // POST de los documentos por GET-AJAX
        [HttpPost]
        public ActionResult FileUploadHandler(int? IDINGRESO)
        {
            // OBTENER EL INGRESO Y EL TIPO DOCUMENTO
            var _entityIngreso = _context.INGRESO.Find(IDINGRESO);

            //tipo doc
            var _entitityTipoDoc = _context.TIPOSDOCUMENTO.Find(1);

            // obtiene todas la lleves de los archivos subidos
            foreach (var fileKey in Request.Files.AllKeys)
            {
                var file = Request.Files[fileKey];
                try
                {
                    if (file != null)
                    {
                        // el nombre del archivo será
                        //fechayhoraActual
                        // fecha de hoy
                        DateTime ahora = DateTime.Now;
                        // captura el long del datetime
                        long ahorita = ahora.Ticks;
                        StringBuilder finalFileName = new StringBuilder();
                        string ahoraFileName = ahorita.ToString();

                        finalFileName.Append(ahoraFileName);
                        finalFileName.Append("_");
                        finalFileName.Append(file.FileName);

                        //var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/App_Data/DocumentosIngreso/" + _entityIngreso.IDPERSONA.ToString() + "/" + _entityIngreso.ID + ""), finalFileName.ToString());
                        file.SaveAs(path);

                        // GUARDAR EL REGISTRO EN LA BASE
                        var _documentosIngreso = new DOCUMENTOSINGRESO
                        {
                            FECHACARGADO = ahora,
                            NOMBRE = _entitityTipoDoc.NOMBRETIPO,
                            IDINGRESO = _entityIngreso.ID,
                            IDTIPODOCUMENTO = _entitityTipoDoc.ID,
                            RUTA = path
                        };

                        _context.DOCUMENTOSINGRESO.Add(_documentosIngreso);
                        _context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { Message = ex.Message });
                }
            }
            return Json(new { Message = "File saved" });
        }

        #region Documentos por ingreso, alternativa
        //[HttpPost]
        //public ActionResult FileUploadHandler(int? IDINGRESO, int? IDTIPODOCUMENTO)
        //{
        //    // OBTENER EL INGRESO Y EL TIPO DOCUMENTO
        //    var _entityIngreso = _context.INGRESO.Find(IDINGRESO);

        //    //tipo doc
        //    var _entitityTipoDoc = _context.TIPOSDOCUMENTO.Find(IDTIPODOCUMENTO);

        //    // obtiene todas la lleves de los archivos subidos
        //    foreach (var fileKey in Request.Files.AllKeys)
        //    {
        //        var file = Request.Files[fileKey];
        //        try
        //        {
        //            if (file != null)
        //            {
        //                // el nombre del archivo será
        //                //fechayhoraActual
        //                // fecha de hoy
        //                DateTime ahora = DateTime.Now;
        //                // captura el long del datetime
        //                long ahorita = ahora.Ticks;
        //                StringBuilder finalFileName = new StringBuilder();
        //                string ahoraFileName = ahorita.ToString();

        //                finalFileName.Append(ahoraFileName);
        //                finalFileName.Append("_");
        //                finalFileName.Append(file.FileName);

        //                //var fileName = Path.GetFileName(file.FileName);
        //                var path = Path.Combine(Server.MapPath("~/App_Data/DocumentosIngreso/" + _entityIngreso.IDPERSONA.ToString() + "/" + _entityIngreso.ID + ""), finalFileName.ToString());
        //                file.SaveAs(path);

        //                // GUARDAR EL REGISTRO EN LA BASE
        //                var _documentosIngreso = new DOCUMENTOSINGRESO
        //                {
        //                    FECHACARGADO = ahora,
        //                    NOMBRE = _entitityTipoDoc.NOMBRETIPO,
        //                    IDINGRESO = _entityIngreso.ID,
        //                    IDTIPODOCUMENTO = _entitityTipoDoc.ID,
        //                    RUTA = path
        //                };

        //                _context.DOCUMENTOSINGRESO.Add(_documentosIngreso);
        //                _context.SaveChanges();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return Json(new { Message = "Error in saving file" });
        //        }
        //    }
        //    return Json(new { Message = "File saved" });
        //}
        #endregion

        //// Consultas dinámicas en get, para JSON y Ajax
        //// Retorno de ciudades por filtro de país
        ////     
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

        // luego que la solicitud es creada, se debe de procesar
        // el procesamiento es, subir de nivel en el flujo de trabajo
        // a bajo nivel, el procesamiento incluye
        // las entradas para edición de cada  uno de los aspectos a evaluar, aspectos y 
        // todas sus dependencias
        
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
            //    //return RedirectToAction("ResultadoProcesamiento", "SolicitudIngreso", new { IDRESUMEN = 1 });
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
                //return RedirectToAction("ResultadoProcesamiento", "SolicitudIngreso", new { IDRESUMEN = 2 });
                return Json(new { success = false, message = "No se ha podido procesar la solocitud" }, JsonRequestBehavior.AllowGet);
            }


        } 
        #endregion

        //Acciones de Solicitudes -- flow 2
        #region Procesamiento Médico

        // index de para ver los pendientes de evaluar
        public async Task<ActionResult> Medicina()
        {
            // obtener las solicitudes de ingreso
            // primero los ingresos
            //get entity
            var _entity = await _context.INGRESO.Include(i => i.FICHA)
                .Where(i => i.STATUSFLOW == 2)
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

        public async Task<ActionResult> EvaluacionMedica(int? ID)
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

            //
            return View(_entity);
        }

        // mediante jquery - json and ajax se actualiza        
        public async Task<ActionResult> GuardarEvaluacionSignosVitales(SIGNOSVITALES _model)
        {
           // sim low conn
            Thread.Sleep(2000);

            try {

                // get entity
                var _entity = await _context.SIGNOSVITALES.FindAsync(_model.IDINGRESO);

                _entity.FRECUENCIACARDIACA = _model.FRECUENCIACARDIACA;
                _entity.PESO = _model.PESO;
                _entity.PRESIONARTERIAL = _model.PRESIONARTERIAL;

                _entity.MEDICORESPONSABLE = _model.MEDICORESPONSABLE;
                _entity.FECHADIAGNOSTICO = DateTime.Now;

                // guarda el conntext
                _context.Entry(_entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                // mesaje de éxito al guardar
                return Json(new { success = true, message = "Solicitud Procesada correctamente!" }, JsonRequestBehavior.AllowGet);

            }catch(Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // actualización de  evaluación médica detalle
        public async Task<ActionResult> GuardarEvaluacionMedicaDetalle(EVALUACIONMEDICADETALLE _model)
        {
            //
            Thread.Sleep(2000);

            try {

                //get entity
                var _entity = await _context.EVALUACIONMEDICADETALLE.FirstOrDefaultAsync(i=>i.IDINGRESO == _model.IDINGRESO && i.IDTIPOEVALUACIONMEDICA == _model.IDTIPOEVALUACIONMEDICA);

                // set entity data               
                _entity.OBSERVACIONES = _model.OBSERVACIONES;

                // actualizar la entity
                _context.Entry(_entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                
                // mesaje de éxito al guardar
                return Json(new { success = true, message = "El registro se ha guardado correctamente!" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // actualización de aparatos sistemas
        public async Task<ActionResult> GuardarAparatosSistemas(APARATOSSISTEMAS _model)
        {
            //
            Thread.Sleep(2000);

            try
            {

                //get entity
                var _entity = await _context.APARATOSSISTEMAS.FirstOrDefaultAsync(i=>i.IDINGRESO == _model.IDINGRESO && i.IDAPARATOSISTEMA == _model.IDAPARATOSISTEMA);

                // set entity data                
                _entity.DIAGNOSTICO = _model.DIAGNOSTICO;                

                // actualizar la entity
                _context.Entry(_entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                // mesaje de éxito al guardar
                return Json(new { success = true, message = "El registro se ha guardado correctamente!" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        //Acciones de Solicitudes -- flow 3
        // procesamiento psquiiatrico
        #region Procesamiento Psquiatrico
        public async Task<ActionResult> Psiquiatria()
        {
            // obtener las solicitudes de ingreso
            // primero los ingresos
            //get entity
            var _entity = await _context.INGRESO.Include(i => i.FICHA)
                .Where(i => i.STATUSFLOW == 3)
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
        
        // get un acceso a nueva Evaluación Psiquiatrica
        public async Task<ActionResult> EvaluacionPsiquiatrica(int? ID)
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

            //
            return View(_entity);
        }

        //acciones de edición de Evaluación Psiquiatrica
        public async Task<ActionResult> GuardarPsqDetalleEnfermedades(PSQ_DETALLEENFERMEDADES _model)
        {
            //
            Thread.Sleep(2000);

            try
            {

                //get entity
                var _entity = await _context.PSQ_DETALLEENFERMEDADES.FindAsync(_model.IDINGRESO);

                // set entity data                
                _entity.DESCRIPCION = _model.DESCRIPCION;

                // actualizar la entity
                _context.Entry(_entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                // mesaje de éxito al guardar
                return Json(new { success = true, message = "El registro se ha guardado correctamente!" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // 
        public async Task<ActionResult> GuardarPsqHistorialIngresoPV(PSQ_HISTORIALINGRESOSPV _model)
        {
            //
            Thread.Sleep(2000);

            try
            {

                //get entity
                var _entity = await _context.PSQ_HISTORIALINGRESOSPV.FindAsync(_model.IDINGRESO);

                // set entity data                
                _entity.DESCRIPCION = _model.DESCRIPCION;
                _entity.HAINGRESADOANTES = _model.HAINGRESADOANTES;

                // actualizar la entity
                _context.Entry(_entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                // mesaje de éxito al guardar
                return Json(new { success = true, message = "El registro se ha guardado correctamente!" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        // guardar cambios o edición de síntomas principales
        public async Task<ActionResult> GuardarPsqSintomasPrincipales(PSQ_SINTOMASPRINCIPALES _model)
        {
            //
            Thread.Sleep(2000);

            try
            {

                //get entity
                var _entity = await _context.PSQ_SINTOMASPRINCIPALES.FindAsync(_model.IDINGRESO);

                // set entity data                
                _entity.DESCRIPCION = _model.DESCRIPCION;

                // actualizar la entity
                _context.Entry(_entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                // mesaje de éxito al guardar
                return Json(new { success = true, message = "El registro se ha guardado correctamente!" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // guardar cambios en observación principal
        public async Task<ActionResult> GuardarPsqObservacionPrincipal(PSQ_OBSERVACIONFINAL _model)
        {
            //
            Thread.Sleep(2000);

            try
            {

                //get entity
                var _entity = await _context.PSQ_OBSERVACIONFINAL.FindAsync(_model.IDINGRESO);

                // set entity data                
                _entity.DESCRIPCION = _model.DESCRIPCION;

                // actualizar la entity
                _context.Entry(_entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                // mesaje de éxito al guardar
                return Json(new { success = true, message = "El registro se ha guardado correctamente!" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // guardar cambios en examen psicopatologico
        public async Task<ActionResult> GuardarPsqExamenPsicoPatologico(PSQ_EXAMENPSICOPATOLOGICO _model)
        {
            //
            Thread.Sleep(2000);

            try
            {

                //get entity
                var _entity = await _context.PSQ_EXAMENPSICOPATOLOGICO.FindAsync(_model.IDINGRESO);

                // set entity data                
                _entity.COOPERA = _model.COOPERA;
                _entity.DETALLECOOPERA = _model.DETALLECOOPERA;
                _entity.TEP = _model.TEP;
                _entity.LENGUAJE = _model.LENGUAJE;
                _entity.DESORDENPENSAMIENTO = _model.DESORDENPENSAMIENTO;
                _entity.INTELECTO = _model.INTELECTO;
                _entity.AFECTO = _model.AFECTO;
                _entity.AUTOCOMPRENSION = _model.AUTOCOMPRENSION;
                _entity.NIVELDROGADICCION = _model.NIVELDROGADICCION;
                _entity.OTROS = _model.OTROS;
                _entity.NEUROLOGICOS = _model.NEUROLOGICOS;
                _entity.DIAGNOSTICO = _model.DIAGNOSTICO;
                _entity.RECOMENDACIONES = _model.RECOMENDACIONES;
                _entity.TRATAMIENTO = _model.TRATAMIENTO;
                

                // actualizar la entity
                _context.Entry(_entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                // mesaje de éxito al guardar
                return Json(new { success = true, message = "El registro se ha guardado correctamente!" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        //Acciones de Solicitudes -- flow 4
        #region Procesamiento Psicología
        //index bandeja de evaluación ps
        public async Task<ActionResult> Psicologia()
        {
            // obtener las solicitudes de ingreso
            // primero los ingresos
            //get entity
            var _entity = await _context.INGRESO.Include(i => i.FICHA)
                .Where(i=>i.STATUSFLOW == 4)
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

       

        // action-controller para vista de Evaluación psicologica
        public async Task<ActionResult> EvaluacionPsicologica(int? ID)
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

            //
            return View(_entity);
        }

        //// Evaluación Psicológica-- 
        //Condición Familiar -- edit
        public async Task<ActionResult> EditarCondicionFamiliar(DATOSSOCIOECONOMICOS _model)
        {
            Thread.Sleep(2000);
            try {
                               
                // get entity
                var _entity = await _context.DATOSSOCIOECONOMICOS.FindAsync(_model.IDINGRESO);

                // SETS
                _entity.CONDICIONFAMILIAR =_model.CONDICIONFAMILIAR;
                _entity.SITUACIONSOACIAL = _model.SITUACIONSOACIAL;

                //guardar el registro
                _context.Entry(_entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();


                return Json(new { success = true, message = "La información ha sido guardada correctamente!" }, JsonRequestBehavior.AllowGet);

            }catch(Exception ex)
            {
               // error = ex.Message;
                //return RedirectToAction("ResultadoProcesamiento", "SolicitudIngreso", new { IDRESUMEN = 2 });
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // actualizar obaservación preliminar
        public async Task<ActionResult> EditarObservacionPreliminar(OBERVACIONPRELIMINAR _model)
        {
            Thread.Sleep(2000);
            try
            {

                // get entity
                var _entity = await _context.OBERVACIONPRELIMINAR.FindAsync(_model.IDINGRESO);

                // SETS
                _entity.PROBLEMAS = _model.PROBLEMAS;
                

                //guardar el registro
                _context.Entry(_entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();


                return Json(new { success = true, message = "La información ha sido guardada correctamente!" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                // error = ex.Message;
                //return RedirectToAction("ResultadoProcesamiento", "SolicitudIngreso", new { IDRESUMEN = 2 });
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //Actualizar historial personal familiar
        public async Task<ActionResult> EditarHistorialPersonalFamiliar(HISTORIALPERSONALFAMILIAR _model)
        {
            Thread.Sleep(2000);
            try
            {

                // get entity
                var _entity = await _context.HISTORIALPERSONALFAMILIAR.FindAsync(_model.IDINGRESO);

                // SETS
                _entity.DESCRIPCION = _model.DESCRIPCION;


                //guardar el registro
                _context.Entry(_entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();


                return Json(new { success = true, message = "La información ha sido guardada correctamente!" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                // error = ex.Message;
                //return RedirectToAction("ResultadoProcesamiento", "SolicitudIngreso", new { IDRESUMEN = 2 });
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // actualizar el estado mental
        public async Task<ActionResult> EditarEstadoMental(ESTADOMENTAL _model)
        {
            Thread.Sleep(2000);
            try
            {

                // get entity
                var _entity = await _context.ESTADOMENTAL.FindAsync(_model.IDINGRESO);

                // SETS
                _entity.DESCRIPCION = _model.DESCRIPCION;


                //guardar el registro
                _context.Entry(_entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();


                return Json(new { success = true, message = "La información ha sido guardada correctamente!" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                // error = ex.Message;
                //return RedirectToAction("ResultadoProcesamiento", "SolicitudIngreso", new { IDRESUMEN = 2 });
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //actualizar impresion diagnostica
        public async Task<ActionResult> EditarImpresionDiagnostica(IMPRESIONDIAGNOSTICA _model)
        {
            Thread.Sleep(2000);
            try
            {

                // get entity
                var _entity = await _context.IMPRESIONDIAGNOSTICA.FindAsync(_model.IDINGRESO);

                // SETS
                _entity.DESCRIPCION = _model.DESCRIPCION;


                //guardar el registro
                _context.Entry(_entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();


                return Json(new { success = true, message = "La información ha sido guardada correctamente!" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                // error = ex.Message;
                //return RedirectToAction("ResultadoProcesamiento", "SolicitudIngreso", new { IDRESUMEN = 2 });
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // actualizar Recomendaciones
        public async Task<ActionResult> EditarRecomendaciones(RECOMENDACIONES _model)
        {
            Thread.Sleep(2000);
            try
            {

                // get entity
                var _entity = await _context.RECOMENDACIONES.FindAsync(_model.IDINGRESO);

                // SETS
                _entity.DESCRIPCION = _model.DESCRIPCION;


                //guardar el registro
                _context.Entry(_entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();


                return Json(new { success = true, message = "La información ha sido guardada correctamente!" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                // error = ex.Message;
                //return RedirectToAction("ResultadoProcesamiento", "SolicitudIngreso", new { IDRESUMEN = 2 });
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // examen psicometrico
        // esta es una partial-view
        //public ActionResult 
        #endregion

        //Acciones Trabajo-Social -- flow 5
        #region Procesamiento Trabajo Social
        public async Task<ActionResult> TrabajoSocial()
        {
            // obtener las solicitudes de ingreso
            // primero los ingresos
            //get entity
            var _entity = await _context.INGRESO.Include(i => i.FICHA)
                .Where(i => i.STATUSFLOW == 5)
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

        //get de evaluación Social
        public async Task<ActionResult> EvaluacionSocial(int? ID)
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

            // set viewModel
            var _viewModel = new INGRESOMODEL();

            _viewModel.ACEPTADO = _entity.ACEPTADO;
            _viewModel.ID = _entity.ID;
            _viewModel.STATUSFLOW = _entity.STATUSFLOW;
            _viewModel.NUMEXPEDIENTE = _entity.NUMEXPEDIENTE;
            _viewModel.OBSERVACIONES = _entity.OBSERVACIONES;
            
            // get utilidades
            _viewModel.DROGAS = await _context.DATOSPROBLEMADROGAS_DROGAS.ToListAsync();
            _viewModel.ESCOLARIDADES = await _context.INFORMACIONACADEMICA_ESCOLARIDAD.ToListAsync();
            _viewModel.OFICIOS = await _context.INFORMACIONACADEMICA_OFICIOS.ToListAsync();
            // luego se comprueba algunos detalles
            //si ese ingreso está activo
            //solicitud de ficha por ingreso
            _viewModel.FICHA = await _context.FICHA.FindAsync(_entity.IDPERSONA);

            _entity.FICHA = await _context.FICHA.FindAsync(_entity.IDPERSONA);
            //Viewbags
            

            // to view
            return View(_entity);
        }

        // JSON para obtener las clasificaciones de Estudios, Oficios Drogas
        //Estudios
        #region Json peticiones
        public ActionResult GetEscolaridades()
        {
            var _escolaridades = from e in _context.INFORMACIONACADEMICA_ESCOLARIDAD
                                 select new
                                 {
                                     e.ID,
                                     e.NOMBREESCOLARIDAD
                                 };
            return Json(_escolaridades, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetTiposDrogas()
        {
            var _drogas = from d in _context.DATOSPROBLEMADROGAS_DROGAS
                          select new { d.ID, d.NOMBRECIENTIFICO };
            return Json(_drogas, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetTiposOficios()
        {
            var _oficios = from o in _context.INFORMACIONACADEMICA_OFICIOS
                           select new
                           {
                               o.ID,
                               o.NOMBREOFICIO
                           };

            return Json(_oficios, JsonRequestBehavior.AllowGet);
        } 

        public ActionResult GetEnfermedades()
        {
            var _enfermedades = from e in _context.CONDICIONFISICA_ENFERMEDADES select new { e.ID, e.NOMBRECIENTIFICO };

            return Json(_enfermedades, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //actualización de datos sobre estudios y base de estudios
        public async Task<ActionResult> GuardarEstudiosEscolaridad(INFORMACIONACADEMICA_ESTUDIOSESCOLARIDAD _model, bool _sabeleer, string[] oficiosSeleccionados)
        {
            // conexión lenta
            Thread.Sleep(2000);

            try {

                // get entity
                var _entity = new INFORMACIONACADEMICA_ESTUDIOSESCOLARIDAD();

                // base de estudios
                var _entityBase = new INFORMACIONACADEMICA_ESTUDIOS();
                _entityBase = await _context.INFORMACIONACADEMICA_ESTUDIOS.FindAsync(_model.IDESTUDIOS);
                _entityBase.SABELEERYESCRIBIR = _sabeleer;                

                // set entity
                _entity.IDESTUDIOS = _model.IDESTUDIOS;
                _entity.IDESCOLARIDAD = _model.IDESCOLARIDAD;
                _entity.DESCRIPCION = _model.DESCRIPCION;

                // guardar los oficios
                var oficiosSelect = new HashSet<string>(oficiosSeleccionados);
                List<INFORMACIONACADEMICA_ESTUDIOSOFICIO> estudiosOficioList = new List<INFORMACIONACADEMICA_ESTUDIOSOFICIO>();

                foreach (var ofi in oficiosSeleccionados)
                {
                    estudiosOficioList.Add(new INFORMACIONACADEMICA_ESTUDIOSOFICIO
                    {
                        IDESTUDIOS = _model.IDESTUDIOS,
                        IDOFICIOS = Int32.Parse(ofi)
                    });
                }

                //set data masiva
                GuardarOficiosIngreso(estudiosOficioList);

                _context = null;
               
                ////save data
                //_context.Entry(_entityBase).State = EntityState.Modified;
                //_context.INFORMACIONACADEMICA_ESTUDIOSESCOLARIDAD.Add(_entity);
                //await _context.SaveChangesAsync(); 
                IDBFactory _dbfactory = new DBFactory();

                var status = _dbfactory.GuardarInfoAcademica(_entityBase, _entity);

                if(status == GuardarInfoAcademicaIngreso.Exito)
                {
                    return Json(new { success = true, message = "Se ha guardado el registro con éxito" }, JsonRequestBehavior.AllowGet);
                }


                return Json(new { success = false, message = MensajeDeErroInfoAcadem(status) }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { success= false, message = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }

        private string MensajeDeErroInfoAcadem(GuardarInfoAcademicaIngreso status)
        {
            string message = "";
            
            switch(status)
            {
                case GuardarInfoAcademicaIngreso.ErorAlGuardarInfoEstudios: message = "Error al guardar informacón académica";
                    return message;
                case GuardarInfoAcademicaIngreso.ErrorAlguardarInfoEstudiosEscolaridad: message = "Error al guardar información académica, estudios formales";
                    return message;
                default: message="error desconocido";
                    return message;
            }

        }

        // add to context masivo
        private void GuardarOficiosIngreso(List<INFORMACIONACADEMICA_ESTUDIOSOFICIO> estudiosOficioList)
        {
            _context = new SICIBD2Entities1();
            _context.Configuration.AutoDetectChangesEnabled = false;

            int count = 0;

            for (int i = 0; i < estudiosOficioList.Count; i++ )
            {
                ++count;
                _context = AddToContext(_context,
                    new INFORMACIONACADEMICA_ESTUDIOSOFICIO {
                    IDESTUDIOS = estudiosOficioList[i].IDESTUDIOS,
                    IDOFICIOS = estudiosOficioList[i].IDOFICIOS},
                    count, estudiosOficioList.Count, true
                    );
            }
        }

        // estrategia de dispose de _context
        private SICIBD2Entities1 AddToContext(SICIBD2Entities1 _context, INFORMACIONACADEMICA_ESTUDIOSOFICIO iNFORMACIONACADEMICA_ESTUDIOSOFICIO, int count, int p1, bool p2)
        {
            _context.Set<INFORMACIONACADEMICA_ESTUDIOSOFICIO>().Add(iNFORMACIONACADEMICA_ESTUDIOSOFICIO);

            if(count % p1 == 0)
            {
                _context.SaveChanges();
                if(p2)
                {
                    _context.Dispose();
                    _context = new SICIBD2Entities1();
                    _context.Configuration.AutoDetectChangesEnabled = false;
                }
            }

            return _context;
        }

        //guardar datos de problemática drogas
        // 1. consumo de drogas
        // vienen todos los tipos de drogas que ha consumido
        public async Task<ActionResult> GuardarDatosProblematicaDrogasConsumo(DATOSPROBLEMADROGAS_CONSUMO _model, string[] drogasSelect)
        {
            // conexión lenta
            Thread.Sleep(2000);

            try {

                // get entity
                var _entity = await _context.DATOSPROBLEMADROGAS_CONSUMO.FindAsync(_model.IDINGRESO);

                // set entity
                _entity.ANIOTRATAMIENTO = _model.ANIOTRATAMIENTO;
                _entity.EDADCOMIENZO = _model.EDADCOMIENZO;
                _entity.EXPLIQUEPROBLEMAS = _model.EXPLIQUEPROBLEMAS;
                _entity.FECHADIAGNOSTICO = DateTime.Now;
                _entity.INTENTODEJARLAS = _model.INTENTODEJARLAS;
                _entity.LUGARTRATAMIENTO = _model.LUGARTRATAMIENTO;
                _entity.PROBLEMAFISICO = _model.PROBLEMAFISICO;
                _entity.PROBLEMAMENTAL = _model.PROBLEMAMENTAL;
                _entity.TRATAMIENTOMEDICO = _model.TRATAMIENTOMEDICO;
                _entity.ABSTINENCIADIAS = _model.ABSTINENCIADIAS;
                _entity.ABSTINENCIAMESES = _model.ABSTINENCIAMESES;
                _entity.ABSTINENCIASEMANAS = _model.ABSTINENCIASEMANAS;
                
                // transformación de datos para tipos de drogas seleccionadas
                var drograsSeleccionadas = new HashSet<string>(drogasSelect);

                //asignación
                List<DATOSPROBLEMADROGAS_CONSUMODROGAS> consumoDrogasList = new List<DATOSPROBLEMADROGAS_CONSUMODROGAS>();

                // set asignación
                foreach(string droga in drograsSeleccionadas)
                {
                    consumoDrogasList.Add(new DATOSPROBLEMADROGAS_CONSUMODROGAS { 
                        IDINGRESO = _model.IDINGRESO,
                        IDDROGA = Int32.Parse(droga)
                    });
                }

                // save data masiva para consumodrogas
                GuardarConsumoDrogas(consumoDrogasList);
                // save data
                _context.Entry(_entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Json(new {success= true, message="Éxito al guardar el registro!"}, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { success = true, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private void GuardarConsumoDrogas(List<DATOSPROBLEMADROGAS_CONSUMODROGAS> consumoDrogasList)
        {
            _context = new SICIBD2Entities1();
            _context.Configuration.AutoDetectChangesEnabled = false;

            int count = 0;

            for (int i = 0; i < consumoDrogasList.Count; i++ )
            {
                ++count;
                _context = AddConsumoDrogasToContext(_context, new DATOSPROBLEMADROGAS_CONSUMODROGAS
                {
                    IDDROGA = consumoDrogasList[i].IDDROGA,
                    IDINGRESO = consumoDrogasList[i].IDINGRESO
                },
                count, 
                consumoDrogasList.Count,                
                true);
            }

        }

        private SICIBD2Entities1 AddConsumoDrogasToContext(SICIBD2Entities1 _context, DATOSPROBLEMADROGAS_CONSUMODROGAS dATOSPROBLEMADROGAS_CONSUMODROGAS, int count, int p1, bool p2)
        {
            _context.Set<DATOSPROBLEMADROGAS_CONSUMODROGAS>().Add(dATOSPROBLEMADROGAS_CONSUMODROGAS);

            if(count %p1 == 0)
            {
                _context.SaveChanges();
                if (p2)
                {
                    _context.Dispose();
                    _context = new SICIBD2Entities1();
                    _context.Configuration.AutoDetectChangesEnabled = false;
                }
            }
            return _context;
        }
        
        //2. datos delictivos
        public async Task<ActionResult> GuardarDatosDelictivos(DATOSDELICTIVOS _model)
        { 
            // conexión lenta
            Thread.Sleep(2000);

            try {
                // get entity
                var _entity = await _context.DATOSDELICTIVOS.FindAsync(_model.IDINGRESO);

                //set entity
                _entity.DESTALLESRECLUSION = _model.DESTALLESRECLUSION;
                _entity.DETALLESACTOS = _model.DETALLESACTOS;
                _entity.FECHADIAGNOSTICO = DateTime.Now;
                _entity.HACOMETIDO = _model.HACOMETIDO;
                _entity.HAESTADOPRESO = _model.HAESTADOPRESO;
                _entity.JUICIOPENDIENTE = _model.JUICIOPENDIENTE;
                
                //save data 
                _context.Entry(_entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Json(new { success=true, message="Éxito al guardar el registro"}, JsonRequestBehavior.AllowGet);

            }catch(Exception ex)
            {
                return Json(new { success=false, message=ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        //Datos sobre condición física
        public async Task<ActionResult> GuardarDatosCondicionFisica(CONDICIONFISICA_INGRESO _model, string[] enfSelec)
        {
            Thread.Sleep(2000);
            try {

                // get entity
                var _entity = await _context.CONDICIONFISICA_INGRESO.FindAsync(_model.IDINGRESO);

                // set entity
                _entity.DETALLE = _model.DETALLE;
                _entity.DUERMEBIEN = _model.DUERMEBIEN;
                _entity.FECHADIAGNOSTICO = DateTime.Now;
                _entity.HASIDOOPERADO = _model.HASIDOOPERADO;
                _entity.ONSERVACIONESGENERALES = _model.ONSERVACIONESGENERALES;
                _entity.PADECE = _model.PADECE;
                _entity.PADECENERVIOS = _model.PADECENERVIOS;
                _entity.RECIBIOTRATAMIENTO = _model.RECIBIOTRATAMIENTO;
                _entity.TIENEIMPEDIMENTOS = _model.TIENEIMPEDIMENTOS;
                
                //enfermedades seleccionadas
                var enfermedadesSeleccionadas = new HashSet<string>(enfSelec);

                //set asignación
                List<CONDICIONFISICA_INTERNOENFERMEDADES> internosEnfermedadesList = new List<CONDICIONFISICA_INTERNOENFERMEDADES>();

                foreach(var enfermedad in enfermedadesSeleccionadas)
                {
                    internosEnfermedadesList.Add(new CONDICIONFISICA_INTERNOENFERMEDADES { 
                        IDINGRESO = _model.IDINGRESO,
                        IDENFERMEDAD = Int32.Parse(enfermedad)
                    });
                }

                //guardar asingnación
                GuardarEnfermedadesInterno(internosEnfermedadesList);

                // save data
                _context.Entry(_entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Éxito al guardar el registro!" }, JsonRequestBehavior.AllowGet);
            }catch(Exception ex)
            {
                return Json(new { success=false, message=ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private void GuardarEnfermedadesInterno(List<CONDICIONFISICA_INTERNOENFERMEDADES> internosEnfermedadesList)
        {
            _context = new SICIBD2Entities1();
            _context.Configuration.AutoDetectChangesEnabled = false;

            int count = 0;

            for (int i = 0; i < internosEnfermedadesList.Count; i++ )
            {
                ++count;
                _context = AddEnfermdadesToContext(
                    _context,
                    new CONDICIONFISICA_INTERNOENFERMEDADES {
                        IDINGRESO = internosEnfermedadesList[i].IDINGRESO,
                        IDENFERMEDAD = internosEnfermedadesList[i].IDENFERMEDAD
                    }, count, internosEnfermedadesList.Count, true
                    );
            }
        }

        private SICIBD2Entities1 AddEnfermdadesToContext(SICIBD2Entities1 _context, CONDICIONFISICA_INTERNOENFERMEDADES cONDICIONFISICA_INTERNOENFERMEDADES, int count, int p1, bool p2)
        {
            _context.Set<CONDICIONFISICA_INTERNOENFERMEDADES>().Add(cONDICIONFISICA_INTERNOENFERMEDADES);

            if(count % p1 == 0)
            {
                _context.SaveChanges();
                if(p2)
                {
                    _context.Dispose();
                    _context = new SICIBD2Entities1();
                    _context.Configuration.AutoDetectChangesEnabled = false;                   

                }
            }
            return _context;
        }

        // motivos de ingreso
        public async Task<ActionResult> GuardarMotivosIngreso(MOTIVOSINGRESO _model)
        {
            Thread.Sleep(2000);
            try {

                // get entity
                var _entity = await _context.MOTIVOSINGRESO.FindAsync(_model.IDINGRESO);

                //set entity
                _entity.FECHADIAGNOSTICO = DateTime.Now;
                _entity.FECHAFIRMAACUERDO = _model.FECHAFIRMAACUERDO;
                _entity.DESCRIPCION = _model.DESCRIPCION;
                _entity.DISPUESTOASUJETARSEPV = _model.DISPUESTOASUJETARSEPV;
                _entity.VOLUNTARIAMENTE = _model.VOLUNTARIAMENTE;

                // save data
                _context.Entry(_entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Éxito al guardar registro!" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { success=false, message=ex.Message}, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion


        #region Pruebas Json 
        //prueba json and jquery data
        public ActionResult DataJson()
        {
            return View();
        }

        // validar registro
        public async Task<ActionResult> ValidarCampo(string _model)
        {
            Thread.Sleep(1000);
            try {
                    
                // si encuentra el registro
                //get entity
                var _entity = await _context.CONDICIONFISICA_ENFERMEDADES.FirstOrDefaultAsync(i=>i.NOMBRECIENTIFICO == _model);

                if(_entity != null)
                {
                    return Json(new { success = false, message = "Ya existe un registro con este nombre" }, JsonRequestBehavior.AllowGet);
                }else{
                    return Json(new { success = true, message = "Go a head!" }, JsonRequestBehavior.AllowGet);
                }
                    
            }catch(Exception ex)
            {
                return Json(new {success=false, message=ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        //post data save
        public ActionResult SaveData(int? ID)
        {
            Thread.Sleep(2000);
            try
            {

                // hace algo
                return Json(new { success = true, message = "se eligió el ID:" + ID + "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = true, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        } 
        #endregion

        //
        #region Prueba de ajax
        //public ActionResult NuevaFichak(FICHAMODEL _model)
        //{
        //    Thread.Sleep(2000);
        //    try
        //    { 
        //        // hacer algo

        //        return Json(new {success=true, message="ddd"}, JsonRequestBehavior.AllowGet);
        //    }catch(Exception ex)
        //    {
        //        return Json(new {success=true, message=ex.Message}, JsonRequestBehavior.AllowGet);
        //    }
        //} 
        #endregion

    }
} 