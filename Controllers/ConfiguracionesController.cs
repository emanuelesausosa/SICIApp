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

            // set model
            List<CENTROTERAPEUTICOMODEL> _modelList = new List<CENTROTERAPEUTICOMODEL>();

            // método asíncrono
        var vCts =   await _centrosTerapeuticos.ToListAsync();

        foreach (CENTROTERAPEUTICO ct in vCts)
            {
                _modelList.Add(new CENTROTERAPEUTICOMODEL { 
                    ID = ct.ID,
                    DESCRIPCION = ct.DESCRIPCION,
                    NOMBRE = ct.NOMBRE, 
                    IDCIUDADOPERACION = ct.IDCIUDADOPERACION
                });
            }
            return View(_modelList);
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

            // vista de filtro para paises y ciudades
            //ViewBag.CODIGOPAIS = new SelectList(_context.COUNTRies, "CODE", "NAME");
            _model.COUNTRIES = _context.COUNTRies;
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
                    IDCIUDADOPERACION = _model.IDCIUDADOPERACION
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

                    return RedirectToAction("CentrosTerapeuticos", "Configuraciones");

                }

                
            }
            // se redirige a la misma página, con el detalle de sus errores
            
            _model.COUNTRIES = _context.COUNTRies;
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
                IDCIUDADOPERACION = _centroTerapeutico.IDCIUDADOPERACION,
                CODIGOPAIS = _context.CITies.Find(_centroTerapeutico.IDCIUDADOPERACION).COUNTRYCODE
            };
                                              


            ViewBag.CODIGOPAIS = new SelectList(_context.COUNTRies, "CODE", "NAME", _model.CODIGOPAIS);
            _model.COUNTRIES = _context.COUNTRies;
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
                var _updateCT = new CENTROTERAPEUTICO();
                _updateCT = await _context.CENTROTERAPEUTICOes.FindAsync(_model.ID);


                _updateCT.NOMBRE = _model.NOMBRE;
                _updateCT.DESCRIPCION = _model.DESCRIPCION;
                _updateCT.IDCIUDADOPERACION = _model.IDCIUDADOPERACION;

                                    
                ;

                // si existe la clave en el contexto
                
                // si el nombre está repetido
                if (_context.CENTROTERAPEUTICOes.FirstOrDefault(c => c.NOMBRE == _model.NOMBRE &&  c.ID != _model.ID) != null)
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
            _model.COUNTRIES = _context.COUNTRies;
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

            // set model
            List<TIPOEVALUACIONMEDICAMODEL> _model = new List<TIPOEVALUACIONMEDICAMODEL>();

            await _tiposem.ToListAsync();

            foreach(TIPOEVALUACIONMEDICA tm in _tiposem)
            {
                _model.Add(new TIPOEVALUACIONMEDICAMODEL { 
                    ID = tm.ID,
                    CATEGORIA = tm.CATEGORIA,
                    DESCRIPCION = tm.DESCRIPCION

                });
            }

            return View(_model);
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
            var _tipoem = await _context.TIPOEVALUACIONMEDICA.FindAsync(ID);

            if(_tipoem == null)
            {
                return HttpNotFound();
            }


            // seteamos el  modelo

            var _model = new TIPOEVALUACIONMEDICAMODEL { 
                 ID = _tipoem.ID,
                 CATEGORIA = _tipoem.CATEGORIA,
                 DESCRIPCION = _tipoem.DESCRIPCION,
                 
            };

            // model to View

            return View(_model);
        }
         
        // Action metodo para crrear un nuevo tipo evaluaci+on médica

        public ActionResult NuevoTipoEvaluacionMedica()
        {
            var _model = new TIPOEVALUACIONMEDICAMODEL();
            return View(_model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NuevoTipoEvaluacionMedica(TIPOEVALUACIONMEDICAMODEL _model)
        {
            // el modelo está validado
            if(ModelState.IsValid)
            {
                //si el nombre ya existe
                if (_context.TIPOEVALUACIONMEDICA.Where(t => t.CATEGORIA == _model.CATEGORIA).Count() >= 1)
                {
                    ModelState.AddModelError(string.Empty, "Este nombre de categoría ya está asignado, por favor elija otro!");
                    return View(_model);
                }
                else
                { 
                    // trasladomos el modelo a entity
                    var _tipoem = new TIPOEVALUACIONMEDICA { 
                    CATEGORIA = _model.CATEGORIA,
                    DESCRIPCION = _model.DESCRIPCION
                    };
                    
                    // se guarda el registro de forma asíncrona
                    _context.TIPOEVALUACIONMEDICA.Add(_tipoem);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("TiposEvaluacionesMedicas", "Configuraciones");
                }
            }
            return View(_model);
        }

        [Authorize]
        public async Task<ActionResult> EditarTipoEvaluacionMedica(int? ID)
        {
            // si el ID es nulo o 0
            if (ID == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //buscamos el ID
            var _tipoem = await _context.TIPOEVALUACIONMEDICA.FindAsync(ID);

            if (_tipoem == null)
            {
                return HttpNotFound();
            }


            // seteamos el  modelo

           var _model = new TIPOEVALUACIONMEDICAMODEL
            {
                ID = _tipoem.ID,
                CATEGORIA = _tipoem.CATEGORIA,
                DESCRIPCION = _tipoem.DESCRIPCION,
                EVALUACIONMEDICADETALLE = _tipoem.EVALUACIONMEDICADETALLE
            };

            // model to View

            return View(_model);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> EditarTipoEvaluacionMedica(TIPOEVALUACIONMEDICAMODEL _model)
        {
            try { 

                if(ModelState.IsValid)
                {
                    //si el nombre ya existe
                    if (_context.TIPOEVALUACIONMEDICA.SingleOrDefault(t => t.CATEGORIA == _model.CATEGORIA && t.ID != _model.ID) != null)
                    {
                        ModelState.AddModelError(string.Empty, "Este nombre de categoría ya está asignado, por favor elija otro!");
                        return View(_model);
                    }
                    else
                    {
                        // trasladomos el modelo a entity
                        var _tipoem = new TIPOEVALUACIONMEDICA();
                        _tipoem = await _context.TIPOEVALUACIONMEDICA.FindAsync(_model.ID);
                            
                            _tipoem.CATEGORIA = _model.CATEGORIA;
                            _tipoem.DESCRIPCION = _model.DESCRIPCION;
                        

                        // se actualiza el registro de forma asíncrona
                        _context.Entry(_tipoem).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                        return RedirectToAction("TiposEvaluacionesMedicas", "Configuraciones");
                    }
                }
            }catch(DbUpdateConcurrencyException ex)
            {

            }catch(RetryLimitExceededException /*dex*/)
            {

            }

            return View();
        }

        #endregion
        
        #region Acciones de Aparatos sistemas

        // todos los aparatos sistemas
        public async Task<ActionResult> AparatosSistemas()
        {
            // from entity database context
            var _aparatos = this._context.APARATOSSISTEMAS_SISTEMAS;

            await _aparatos.ToListAsync();

            //to Model
            List<APARATOSSISTEMAS_SISTEMASMODEL> _model = new List<APARATOSSISTEMAS_SISTEMASMODEL>();

            foreach(APARATOSSISTEMAS_SISTEMAS aparato in _aparatos)
            {
                _model.Add(new APARATOSSISTEMAS_SISTEMASMODEL { 
                     ID = aparato.ID,
                     NOMBRE = aparato.NOMBRE,
                     DESCRIPCION = aparato.DESCRIPCION,
                });
            }

            // a  la vista
            return View(_model);
        }
            

        // detalle de aparato sistema 
        public async Task<ActionResult> DetalleAparatoSistema(int? ID)
        {
           if(ID == 0)
           {
               return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
           }

            // buecamos el entity
           var _aparatoSistema = await this._context.APARATOSSISTEMAS_SISTEMAS.FindAsync(ID);

            if(_aparatoSistema == null)
            {
                return HttpNotFound();
            }

            // set to model
            var _model = new APARATOSSISTEMAS_SISTEMASMODEL { 
                    ID = _aparatoSistema.ID,
                    NOMBRE = _aparatoSistema.NOMBRE,
                    DESCRIPCION = _aparatoSistema.DESCRIPCION,
                    
            };

            // model to view
            return View(_model);
        }

        // Crea un nuevo registro
        

        // GET
        public ActionResult NuevoAparatoSistema()
        {
            //
            var _model = new APARATOSSISTEMAS_SISTEMASMODEL();
            return View(_model);
        }


        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NuevoAparatoSistema(APARATOSSISTEMAS_SISTEMASMODEL _model)
        {
            if(ModelState.IsValid)
            {
                // si existe un nombre igual
                if(_context.APARATOSSISTEMAS_SISTEMAS.SingleOrDefault(a=>a.NOMBRE == _model.NOMBRE) != null)
                {
                    ModelState.AddModelError(string.Empty, "Ya existe un registro con este mismo nombre, por favor elija uno diferente");
                    return View(_model);
                }

                // from model to entity
                var _aparatosistema = new APARATOSSISTEMAS_SISTEMAS { 
                NOMBRE = _model.NOMBRE,
                DESCRIPCION = _model.DESCRIPCION
                };
                // guardar el registro
                _context.APARATOSSISTEMAS_SISTEMAS.Add(_aparatosistema);
                await _context.SaveChangesAsync();

                return RedirectToAction("AparatosSistemas", "Configuraciones");
            }

            return View(_model);
        }

        //editar el registro
        // GET
        public async Task<ActionResult> EditarAparatoSistema(int? ID)
        {
            if (ID == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // buecamos el entity
            var _aparatoSistema = await this._context.APARATOSSISTEMAS_SISTEMAS.FindAsync(ID);

            if (_aparatoSistema == null)
            {
                return HttpNotFound();
            }

            // set to model
            var _model = new APARATOSSISTEMAS_SISTEMASMODEL
            {
                ID = _aparatoSistema.ID,
                NOMBRE = _aparatoSistema.NOMBRE,
                DESCRIPCION = _aparatoSistema.DESCRIPCION,
                APARATOSSISTEMAS = _aparatoSistema.APARATOSSISTEMAS
            };

            // model to view
            return View(_model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditarAparatoSistema(APARATOSSISTEMAS_SISTEMASMODEL _model)
        {
            if (ModelState.IsValid)
            {
                // si existe un nombre igual
                if (_context.APARATOSSISTEMAS_SISTEMAS.SingleOrDefault(a => a.NOMBRE == _model.NOMBRE && a.ID != _model.ID) != null)
                {
                    ModelState.AddModelError(string.Empty, "Ya existe un registro con este mismo nombre, por favor elija uno diferente");
                    return View(_model);
                }

                // from model to entity

                var _aparatosistema = new APARATOSSISTEMAS_SISTEMAS();
                _aparatosistema = await _context.APARATOSSISTEMAS_SISTEMAS.FindAsync(_model.ID);
                    _aparatosistema.ID = _model.ID;
                    _aparatosistema.NOMBRE = _model.NOMBRE;
                    _aparatosistema.DESCRIPCION = _model.DESCRIPCION;
                
                // guardar el registro
                _context.Entry(_aparatosistema).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return RedirectToAction("AparatosSistemas", "Configuraciones");
            }

            return View(_model);
        }
        #endregion

        // seccion de mantenimiento  de tipo documento
        #region Acciones Mant, tipos Documentos
        
        // todos los tipos de documentos
        public async Task<ActionResult> TiposDocumentos()
        {
            // get entity 
            var _entity = await _context.TIPOSDOCUMENTO.ToListAsync();

            // get list model
            List<TIPOSDOCUMENTOMODEL> _model = new List<TIPOSDOCUMENTOMODEL>();

            //set model 
            foreach(TIPOSDOCUMENTO documentos in _entity)
           {
               _model.Add(new TIPOSDOCUMENTOMODEL { 
                    ID = documentos.ID,
                    NOMBRETIPO = documentos.NOMBRETIPO,
                    DESCRIPCION = documentos.DESCRIPCION
               });
            }

            // to view
            return View(_model);

        }


        // detalle documento
        public async Task<ActionResult> DetalleTipoDocumento(int? ID)
        {
            if(ID ==  null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //set _entity
            var _enitity = await _context.TIPOSDOCUMENTO.FindAsync(ID);


            // si es nulo
            if(_enitity == null)
            {
                return HttpNotFound();
            }

           // set model
            var _model = new TIPOSDOCUMENTOMODEL { 
                ID = _enitity.ID,
                NOMBRETIPO = _enitity.NOMBRETIPO,
                DESCRIPCION = _enitity.DESCRIPCION
            };

            // to view
            return View(_model);
        }

        // nuevo tipo documento
        // GET
        public ActionResult NuevoTipoDucumento()
        {
            // set model
            var _model = new TIPOSDOCUMENTOMODEL();
            return View(_model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NuevoTipoDucumento(TIPOSDOCUMENTOMODEL _model)
        {
            if(ModelState.IsValid)
            {
                // veriifcar el nombre no se repita
                if(_context.TIPOSDOCUMENTO.SingleOrDefault(d=>d.NOMBRETIPO == _model.NOMBRETIPO) !=  null)
                {
                    ModelState.AddModelError("", "El nombre de este tipo de documento ya existe, por facvor elija otro ");
                    return View(_model);
                }else
                {
                    try{

                        // get entity
                        var _entity = new TIPOSDOCUMENTO{
                            NOMBRETIPO = _model.NOMBRETIPO,
                            DESCRIPCION = _model.DESCRIPCION
                        };
                        // guardar el registro
                        _context.TIPOSDOCUMENTO.Add(_entity);
                        await _context.SaveChangesAsync();
                        
                        // redireccionar al índice
                        return RedirectToAction("TiposDocumentos","Configuraciones");

                    }catch(Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
                    }
                }

            }
            return View(_model);
        }

        // Edicíón de tipos documento
        // GET
        public async Task<ActionResult> EditarTipoDocumento(int? ID)
        {

            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //set _entity
            var _enitity = await _context.TIPOSDOCUMENTO.FindAsync(ID);
            

            // si es nulo
            if (_enitity == null)
            {
                return HttpNotFound();
            }

            // set model
            var _model = new TIPOSDOCUMENTOMODEL
            {
                ID = _enitity.ID,
                NOMBRETIPO = _enitity.NOMBRETIPO,
                DESCRIPCION = _enitity.DESCRIPCION
            };

            // to view
            return View(_model);
        }

        // POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditarTipoDocumento(TIPOSDOCUMENTOMODEL _model)
        {
            if (ModelState.IsValid)
            {
                // veriifcar el nombre no se repita
                if (_context.TIPOSDOCUMENTO.SingleOrDefault(d => d.NOMBRETIPO == _model.NOMBRETIPO && d.ID != _model.ID) != null)
                {
                    ModelState.AddModelError("", "El nombre de este tipo de documento ya existe, por favor elija otro ");
                    return View(_model);
                }
                else
                {
                    try
                    {

                        // get entity
                        var _entity = new TIPOSDOCUMENTO();
                        _entity = await _context.TIPOSDOCUMENTO.FindAsync(_model.ID);                        
                            _entity.NOMBRETIPO = _model.NOMBRETIPO;
                            _entity.DESCRIPCION = _model.DESCRIPCION;
                        
                        // guardar el registro
                            _context.Entry(_entity).State = EntityState.Modified;
                            await _context.SaveChangesAsync();

                        // redireccionar al índice
                        return RedirectToAction("TiposDocumentos", "Configuraciones");

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
                    }
                }

            }
            return View(_model);
        }

        #endregion

        // seccion de mantenimiento de causas egreso
        #region Acciones de mant. Causas egreso
        // todas las tipos causas egreso
        public async  Task<ActionResult> CausasEgreso()
        {
            //get entity
            var _entity = await _context.CAUSASEGRESO.ToListAsync();

            // get _model
            List<CAUSASEGRESOMODEL> _model = new List<CAUSASEGRESOMODEL>();

            // set model
            foreach(CAUSASEGRESO causas in _entity)            
            {
                _model.Add(new CAUSASEGRESOMODEL { 
                    ID = causas.ID,
                    NOMBRE = causas.NOMBRE,
                    DESCRIPCION = causas.DESCRIPCION
                });
            }

            // to view
            return View(_model);
        }

        // detalles causas egresos
        public async Task<ActionResult> DetalleCausaEgreso(int? ID)
        {
            // bad request
            if(ID == null)            
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // get entity
            var _entity = await _context.CAUSASEGRESO.FindAsync(ID);

            // set model
            var _model = new CAUSASEGRESOMODEL {
                ID = _entity.ID,
                NOMBRE = _entity.NOMBRE,
                DESCRIPCION = _entity.DESCRIPCION
            };

            // to view
            return View(_model);
        }

        // Nueva causa egreso
        // GET
        public ActionResult NuevaCausaEgreso()
        {
            // get model 
            var _model = new CAUSASEGRESOMODEL();

            // toV view
            return View(_model);
        }

        // POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NuevaCausaEgreso(CAUSASEGRESOMODEL _model)
        {
            if(ModelState.IsValid)
            {
                // comprobar el nombre
                if(_context.CAUSASEGRESO.FirstOrDefault(c=>c.NOMBRE == _model.NOMBRE) != null)
                {
                    ModelState.AddModelError("","Este nombre de Causa ya está asignado a otro registro, por favir elija otro");
                    return View(_model);
                }else{

                try{

                    // get entity
                    var _entity = new CAUSASEGRESO { 
                        NOMBRE = _model.NOMBRE,
                        DESCRIPCION = _model.DESCRIPCION
                    };

                    // guardar el registro
                    _context.CAUSASEGRESO.Add(_entity);
                    await _context.SaveChangesAsync();

                    //redireccionar

                    return RedirectToAction("CausasEgreso","Configuraciones");

                }catch(Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
                }
            }
            return View(_model);
        }



        // Edición de Causas egreso
        //GET

        public async Task<ActionResult> EditarCausaEgreso(int? ID)
        {
            // bad request
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // get entity
            var _entity = await _context.CAUSASEGRESO.FindAsync(ID);

            // set model
            var _model = new CAUSASEGRESOMODEL
            {
                ID = _entity.ID,
                NOMBRE = _entity.NOMBRE,
                DESCRIPCION = _entity.DESCRIPCION
            };

            // to view
            return View(_model);
        }

        

        // POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditarCausaEgreso(CAUSASEGRESOMODEL _model)
        {
            if (ModelState.IsValid)
            {
                // comprobar el nombre
                if (_context.CAUSASEGRESO.FirstOrDefault(c => c.NOMBRE == _model.NOMBRE && c.ID != _model.ID) != null)
                {
                    ModelState.AddModelError("", "Este nombre de Causa ya está asignado a otro registro, por favir elija otro");
                    return View(_model);
                }
                else
                {

                    try
                    {

                        // get entity
                        var _entity = new CAUSASEGRESO();
                        _entity = await _context.CAUSASEGRESO.FindAsync(_model.ID);

                        _entity.NOMBRE = _model.NOMBRE;
                        _entity.DESCRIPCION = _model.DESCRIPCION;
                        

                        // actualizar el registro
                        _context.Entry(_entity).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                        //redireccionar

                        return RedirectToAction("CausasEgreso", "Configuraciones");

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
                    }
                }
            }
            return View(_model);
        }

        #endregion

        //mantenimiento datos problemas drogas
        #region Acciones tipos drogas
        // Todos los tipos drogas

        public async Task<ActionResult> TiposDrogas()
        {
            //get entity 
            var _entity = await _context.DATOSPROBLEMADROGAS_DROGAS.ToListAsync();

            // get model
            List<DATOSPROBLEMADROGAS_DROGASMODEL> _model = new List<DATOSPROBLEMADROGAS_DROGASMODEL>();

            // set model
            foreach (DATOSPROBLEMADROGAS_DROGAS drogas in _entity)
            {
                _model.Add(new DATOSPROBLEMADROGAS_DROGASMODEL
                {
                    ID = drogas.ID,
                    NOMBRECIENTIFICO = drogas.NOMBRECIENTIFICO,
                    NOMBRECOMUN = drogas.NOMBRECOMUN,
                    DESCRIPCION = drogas.DESCRIPCION
                });
            }

            // to view
            return View(_model);
        }


        // detalles tipos de drogas
        public async Task<ActionResult> DetallesTiposDrogas(int? ID)
        {
            // Bad request
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // get entity
            var _entity = await _context.DATOSPROBLEMADROGAS_DROGAS.FindAsync(ID);

            //si no existe
            if (_entity == null)
            {
                return HttpNotFound();
            }

            // set model
            var _model = new DATOSPROBLEMADROGAS_DROGASMODEL
            {
                ID = _entity.ID,
                NOMBRECIENTIFICO = _entity.NOMBRECIENTIFICO,
                NOMBRECOMUN = _entity.NOMBRECOMUN,
                DESCRIPCION = _entity.DESCRIPCION
            };

            // to view
            return View(_model);
        }


        // nuevo tipo droga
        //GET
        public ActionResult NuevoTipoDroga()
        {
            // get model
            var _model = new DATOSPROBLEMADROGAS_DROGASMODEL();

            // to view
            return View(_model);
        }

        //POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NuevoTipoDroga(DATOSPROBLEMADROGAS_DROGASMODEL _model)
        {
            if (ModelState.IsValid)
            {
                // comprobar que el nombre científico y el común
                if (_context.DATOSPROBLEMADROGAS_DROGAS.FirstOrDefault(d => d.NOMBRECIENTIFICO == _model.NOMBRECIENTIFICO || d.NOMBRECOMUN == _model.NOMBRECOMUN) != null)
                {
                    ModelState.AddModelError("", "El nombre científico o el común ya está asignado a otro registro, por favor elija otros");
                    return View(_model);
                }
                else
                {
                    try
                    {

                        // set entity
                        var _entity = new DATOSPROBLEMADROGAS_DROGAS
                        {
                            NOMBRECIENTIFICO = _model.NOMBRECIENTIFICO,
                            NOMBRECOMUN = _model.NOMBRECOMUN,
                            DESCRIPCION = _model.DESCRIPCION
                        };

                        //guardar el registro
                        _context.DATOSPROBLEMADROGAS_DROGAS.Add(_entity);
                        await _context.SaveChangesAsync();

                        // redireccionar
                        return RedirectToAction("TiposDrogas", "Configuraciones");

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
                    }
                }

            }
            return View(_model);
        }

        // edición de tipos drogas
        // GET
        public async Task<ActionResult> EdicionTiposDrogas(int? ID)
        {
            // Bad request
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // get entity
            var _entity = await _context.DATOSPROBLEMADROGAS_DROGAS.FindAsync(ID);

            //si no existe
            if (_entity == null)
            {
                return HttpNotFound();
            }

            // set model
            var _model = new DATOSPROBLEMADROGAS_DROGASMODEL
            {
                ID = _entity.ID,
                NOMBRECIENTIFICO = _entity.NOMBRECIENTIFICO,
                NOMBRECOMUN = _entity.NOMBRECOMUN,
                DESCRIPCION = _entity.DESCRIPCION
            };

            // to view
            return View(_model);
        }

        //POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EdicionTipoDroga(DATOSPROBLEMADROGAS_DROGASMODEL _model)
        {
            if (ModelState.IsValid)
            {
                // comprobar que el nombre científico y el común
                if (_context.DATOSPROBLEMADROGAS_DROGAS.FirstOrDefault(d => d.NOMBRECIENTIFICO == _model.NOMBRECIENTIFICO || d.NOMBRECOMUN == _model.NOMBRECOMUN && d.ID != _model.ID) != null)
                {
                    ModelState.AddModelError("", "El nombre científico o el común ya está asignado a otro registro, por favor elija otros");
                    return View(_model);
                }
                else
                {
                    try
                    {

                        // set entity
                        var _entity = new DATOSPROBLEMADROGAS_DROGAS();
                        _entity = await _context.DATOSPROBLEMADROGAS_DROGAS.FindAsync(_model.ID);

                        _entity.NOMBRECIENTIFICO = _model.NOMBRECIENTIFICO;
                        _entity.NOMBRECOMUN = _model.NOMBRECOMUN;
                        _entity.DESCRIPCION = _model.DESCRIPCION;


                        //actualizar el registro
                        _context.Entry(_entity).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                        // redireccionar
                        return RedirectToAction("TiposDrogas", "Configuraciones");

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
                    }
                }

            }
            return View(_model);
        } 
        #endregion
    }
}