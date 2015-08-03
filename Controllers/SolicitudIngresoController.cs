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
            var _entity = await _context.INGRESO.Include(i => i.FICHA)
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
                    return Json(new { Message = "Error in saving file" });
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


    }
}