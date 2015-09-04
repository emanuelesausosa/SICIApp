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
                Console.WriteLine(ex.StackTrace);
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

        // seccion de mantenimiento  de tipo documento -- YA --sp map -- view created -- modif y probada
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

        // seccion de mantenimiento de causas egreso -- YA -- sp map -- view created -- modif y probada
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

        //mantenimiento datos problemas drogas -- ya -- sp map -- view created -- modif y probada **
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
        public async Task<ActionResult> EdicionTiposDrogas(DATOSPROBLEMADROGAS_DROGASMODEL _model)
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

        // manteniniento de exámenes psicimpetricos -- YA -- sp map --view created -- modif y probada *** subir archivo
        #region Acciones mant. de Exámenes psicimétricos

        // todos los exámenes
        public async Task<ActionResult> TiposExamenesPSmetricos()
        {
            // get entity
            var _entity = await _context.EXAMENPSICOMETRICO_EXAMEN.ToListAsync();

            // get _model
            List<EXAMENPSICOMETRICO_EXAMENMODEL> _model = new List<EXAMENPSICOMETRICO_EXAMENMODEL>();

            // set model
            foreach(EXAMENPSICOMETRICO_EXAMEN examen in _entity)
            {
                _model.Add(new EXAMENPSICOMETRICO_EXAMENMODEL { 
                    IDEXAMEN = examen.IDEXAMEN,
                    TITULO = examen.TITULO,
                    DESCRIPCION = examen.DESCRIPCION,
                    ARCHIVOFISOCO = examen.ARCHIVOFISOCO
                });
            }

            // to view
            return View(_model);
        }

        // detalles Examen psicometrico - Examen
        public async Task<ActionResult> DetalleExamenPsmetrico(int? ID)
        {
            if(ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            // get entity
            var _entity = await _context.EXAMENPSICOMETRICO_EXAMEN.FindAsync(ID);

            // validación entity/
            if(_entity == null)
            {
                return HttpNotFound();
            }


            //set model
            var _model = new EXAMENPSICOMETRICO_EXAMENMODEL { 
                IDEXAMEN = _entity.IDEXAMEN,
                TITULO = _entity.TITULO,
                DESCRIPCION = _entity.DESCRIPCION,
                ARCHIVOFISOCO = _entity.ARCHIVOFISOCO
            };

            // to view
            return View(_model);

        }

        // nuevo examen ps- métrico
        // GET
        public ActionResult NuevoExamenPSmetrico()
        {
            // get model
            var _model = new EXAMENPSICOMETRICO_EXAMENMODEL();
            return View(_model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NuevoExamenPSmetrico(EXAMENPSICOMETRICO_EXAMENMODEL _model)
        {
            if(ModelState.IsValid)
            {
                // comprobar el título
                if (_context.EXAMENPSICOMETRICO_EXAMEN.FirstOrDefault(e => e.TITULO == _model.TITULO) != null)
                {
                    ModelState.AddModelError("", "Este título ya está asignado, por favor elija otro");
                    return View();
                }
                else
                {
                    try { 

                        // set entity
                        var _entity = new EXAMENPSICOMETRICO_EXAMEN { 
                            TITULO = _model.TITULO,
                            DESCRIPCION = _model.DESCRIPCION,
                            ARCHIVOFISOCO = _model.ARCHIVOFISOCO
                        };

                        // guardar registro
                        _context.EXAMENPSICOMETRICO_EXAMEN.Add(_entity);
                        await _context.SaveChangesAsync();

                        // redireccionar
                        return RedirectToAction("TiposExamenesPSmetricos", "Configuraciones");
                    }catch(Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
                    }
                }
            }
            return View(_model);
        }

        // edición de Exámenes PS
        // GET
        public async Task<ActionResult> EditarExamenPsmetrico(int? ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            // get entity
            var _entity = await _context.EXAMENPSICOMETRICO_EXAMEN.FindAsync(ID);

            // validación entity/
            if (_entity == null)
            {
                return HttpNotFound();
            }


            //set model
            var _model = new EXAMENPSICOMETRICO_EXAMENMODEL
            {
                IDEXAMEN = _entity.IDEXAMEN,
                TITULO = _entity.TITULO,
                DESCRIPCION = _entity.DESCRIPCION,
                ARCHIVOFISOCO = _entity.ARCHIVOFISOCO
            };

            // to view
            return View(_model);

        }        

        //POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditarExamenPSmetrico(EXAMENPSICOMETRICO_EXAMENMODEL _model)
        {
            if (ModelState.IsValid)
            {
                // comprobar el título
                if (_context.EXAMENPSICOMETRICO_EXAMEN.FirstOrDefault(e => e.TITULO == _model.TITULO && e.IDEXAMEN != _model.IDEXAMEN) != null)
                {
                    ModelState.AddModelError("", "Este título ya está asignado, por favor elija otro");
                    return View();
                }
                else
                {
                    try
                    {

                        // set entity
                        var _entity = new EXAMENPSICOMETRICO_EXAMEN();
                        _entity = await _context.EXAMENPSICOMETRICO_EXAMEN.FindAsync(_model.IDEXAMEN);
                            _entity.TITULO = _model.TITULO;
                            _entity.DESCRIPCION = _model.DESCRIPCION;
                            _entity.ARCHIVOFISOCO = _model.ARCHIVOFISOCO;
                        

                        // actualizar registro
                            _context.Entry(_entity).State = EntityState.Modified;
                            await _context.SaveChangesAsync();

                        // redireccionar
                        return RedirectToAction("TiposExamenesPSmetricos", "Configuraciones");
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

        // acciones de mantenimiento de examen ps metrico - contenido --YA -- sp map ---- view created -- modif y probada
        #region Acciones de Mant. De Exámenes ps-métricos - contenido
        // todos los contenidos
        public async Task<ActionResult> ContenidosExamenes()
        {
            // get _entity
            var _entity = await _context.EXAMENPSICOMETRICO_EXAMENCONTENIDO.ToListAsync();

            // get model
            List<EXAMENPSICOMETRICO_EXAMENCONTENIDOMODEL> _model = new List<EXAMENPSICOMETRICO_EXAMENCONTENIDOMODEL>();

            // set _model

            foreach(EXAMENPSICOMETRICO_EXAMENCONTENIDO contenido in _entity)
            {
                _model.Add(new EXAMENPSICOMETRICO_EXAMENCONTENIDOMODEL { 
                    IDEXAMEN = contenido.IDEXAMEN,
                    IDPREGUNTA = contenido.IDPREGUNTA,
                    DESCRIPCION = contenido.DESCRIPCION,
                    DETALLE = contenido.DETALLE,
                    VALORACION = contenido.VALORACION
                });
            }

            // to view
            return View(_model);
        }

        // detalle de contenidio
        public async Task<ActionResult> DetalleContenidoExamen(int? ID)
        {
           // BAD REQUEST
            if(ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            // get entity
            var _entity = await _context.EXAMENPSICOMETRICO_EXAMENCONTENIDO.FindAsync(ID);

            // si entity no existe
            if(_entity  == null)
            {
                return HttpNotFound();
            }

            // set model
            var _model = new EXAMENPSICOMETRICO_EXAMENCONTENIDOMODEL {
                IDEXAMEN = _entity.IDEXAMEN,
                IDPREGUNTA = _entity.IDPREGUNTA,
                DESCRIPCION = _entity.DESCRIPCION,
                DETALLE = _entity.DETALLE,
                VALORACION = _entity.VALORACION
            };

            //model to view
            return View(_model);
        }


        // Nuevo Contenido examen
        public ActionResult NuevoContenidoExamen()
        {
            // get model
            var _model = new EXAMENPSICOMETRICO_EXAMENCONTENIDOMODEL();
            ViewBag.IDEXAMEN = new SelectList(_context.EXAMENPSICOMETRICO_EXAMEN, "IDEXAMEN", "TITULO");
            return View(_model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NuevoContenidoExamen(EXAMENPSICOMETRICO_EXAMENCONTENIDOMODEL _model)
        {
            if(ModelState.IsValid)
            {
                try {

                    // get entity
                    var _entity = new EXAMENPSICOMETRICO_EXAMENCONTENIDO {
                        IDEXAMEN = _model.IDEXAMEN,                        
                        DESCRIPCION = _model.DESCRIPCION,
                        DETALLE = _model.DETALLE,
                        VALORACION = _model.VALORACION
                    };

                    // guardar cambioes
                    _context.EXAMENPSICOMETRICO_EXAMENCONTENIDO.Add(_entity);
                    await _context.SaveChangesAsync();

                    // redirecconar
                    return RedirectToAction("ContenidosExamenes", "Configuraciones");
                }catch(Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }

            ViewBag.IDEXAMEN = new SelectList(_context.EXAMENPSICOMETRICO_EXAMEN, "IDEXAMEN", "TITULO", _model.IDEXAMEN);
            return View(_model);
        }

        // edición de contenidos 
        // GET
        public async Task<ActionResult> EditarContenidoExamen(int? ID)
        {
            // BAD REQUEST
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            // get entity
            var _entity = await _context.EXAMENPSICOMETRICO_EXAMENCONTENIDO.FindAsync(ID);

            // si entity no existe
            if (_entity == null)
            {
                return HttpNotFound();
            }

            // set model
            var _model = new EXAMENPSICOMETRICO_EXAMENCONTENIDOMODEL
            {
                IDEXAMEN = _entity.IDEXAMEN,
                IDPREGUNTA = _entity.IDPREGUNTA,
                DESCRIPCION = _entity.DESCRIPCION,
                DETALLE = _entity.DETALLE,
                VALORACION = _entity.VALORACION
            };

            //model to view
            ViewBag.IDEXAMEN = new SelectList(_context.EXAMENPSICOMETRICO_EXAMEN, "IDEXAMEN", "TITULO");
            return View(_model);
        }



        // POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditarContenidoExamen(EXAMENPSICOMETRICO_EXAMENCONTENIDOMODEL _model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    // get entity
                    var _entity = new EXAMENPSICOMETRICO_EXAMENCONTENIDO();
                    _entity = await _context.EXAMENPSICOMETRICO_EXAMENCONTENIDO.FindAsync(_model.IDPREGUNTA);
                        _entity.IDEXAMEN = _model.IDEXAMEN;
                        _entity.DESCRIPCION = _model.DESCRIPCION;
                        _entity.DETALLE = _model.DETALLE;
                        _entity.VALORACION = _model.VALORACION;

                    // actualizar cambios
                    _context.Entry(_entity).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    // redireccionar
                    return RedirectToAction("ContenidosExamenes", "Configuraciones");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }

            ViewBag.IDEXAMEN = new SelectList(_context.EXAMENPSICOMETRICO_EXAMEN, "IDEXAMEN", "TITULO", _model.IDEXAMEN);
            return View(_model);
        }
        #endregion

        // acciones para mantenimeiento de enfermendades -- YA -- sp map -- view created -- modif y probada *** subir archivo
        #region Acciones mant. de enfermades
        // Todas las enfermedades
        public async Task<ActionResult> Enfermedades()
        {
            // get entity
            var _entity = await _context.CONDICIONFISICA_ENFERMEDADES.ToListAsync();

            // get model
            List<CONDICIONFISICA_ENFERMEDADESMODEL> _model = new List<CONDICIONFISICA_ENFERMEDADESMODEL>();

            // set model
            foreach(CONDICIONFISICA_ENFERMEDADES enfermedades in _entity )
            {
                _model.Add(new CONDICIONFISICA_ENFERMEDADESMODEL { 
                    ID = enfermedades.ID,
                    NOMBRECIENTIFICO = enfermedades.NOMBRECIENTIFICO,
                    NOMBRESINOMIMO = enfermedades.NOMBRESINOMIMO,
                    DESCRIPCION = enfermedades.DESCRIPCION

                });
            }

            // TO VIEW
            return View(_model);        
        }

        // Detalle enfermedad
        public async Task<ActionResult> DetalleEnfermedad(int? ID)
        {
            // BAD REQUEST
            if(ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            // get entity
            var _entity = await _context.CONDICIONFISICA_ENFERMEDADES.FindAsync(ID);

            // si entity no existe
            if(_entity  == null)
            {
                return HttpNotFound();
            }


            // get model
            var _model = new CONDICIONFISICA_ENFERMEDADESMODEL {
                ID = _entity.ID,
                NOMBRECIENTIFICO = _entity.NOMBRECIENTIFICO,
                NOMBRESINOMIMO = _entity.NOMBRESINOMIMO,
                DESCRIPCION = _entity.DESCRIPCION
            };
            return View(_model);
        }


        //Nueva enfermedad
        // GET
        public ActionResult NuevaEnfermedad()
        {
           // get model
            var _model = new CONDICIONFISICA_ENFERMEDADESMODEL();
            ///return View(_model);
            ///
            return PartialView("_NuevaEnfermedad", _model);
        }

        // POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NuevaEnfermedad(CONDICIONFISICA_ENFERMEDADESMODEL _model)
        {
            if(ModelState.IsValid)
            {
                // valdición de nombres repetidos
                if (_context.CONDICIONFISICA_ENFERMEDADES.FirstOrDefault(e => e.NOMBRECIENTIFICO == _model.NOMBRECIENTIFICO || e.NOMBRESINOMIMO == _model.NOMBRESINOMIMO) != null)
                {
                    ModelState.AddModelError("", "El nombre científico o el sinónimo ya existen para otro registro, por favor elija otro");
                    return View(_model);
                }
                else
                { 
                    // get entity
                    var _entity = new CONDICIONFISICA_ENFERMEDADES {
                        NOMBRECIENTIFICO = _model.NOMBRECIENTIFICO,
                        NOMBRESINOMIMO = _model.NOMBRESINOMIMO,
                        DESCRIPCION = _model.DESCRIPCION
                    };

                    // guardar registro
                    _context.CONDICIONFISICA_ENFERMEDADES.Add(_entity);
                    await _context.SaveChangesAsync();

                    // redireccionar al índice
                    return RedirectToAction("Enfermedades", "Configuraciones");

                }
            }            
            // to view
            return View(_model);
        }

        // editar enfermedad
        public async Task<ActionResult> EditarEnfermedad(int? ID)
        {
            // BAD REQUEST
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            // get entity
            var _entity = await _context.CONDICIONFISICA_ENFERMEDADES.FindAsync(ID);

            // si entity no existe
            if (_entity == null)
            {
                return HttpNotFound();
            }


            // get model
            var _model = new CONDICIONFISICA_ENFERMEDADESMODEL
            {
                ID = _entity.ID,
                NOMBRECIENTIFICO = _entity.NOMBRECIENTIFICO,
                NOMBRESINOMIMO = _entity.NOMBRESINOMIMO,
                DESCRIPCION = _entity.DESCRIPCION
            };
            return View(_model);
        }

        
        // POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditarEnfermedad(CONDICIONFISICA_ENFERMEDADESMODEL _model)
        {
            if (ModelState.IsValid)
            {
                // valdición de nombres repetidos
                if (_context.CONDICIONFISICA_ENFERMEDADES.FirstOrDefault(e => e.NOMBRECIENTIFICO == _model.NOMBRECIENTIFICO || e.NOMBRESINOMIMO == _model.NOMBRESINOMIMO && e.ID != _model.ID) != null)
                {
                    ModelState.AddModelError("", "El nombre científico o el sinónimo ya existen para otro registro, por favor elija otro");
                    return View(_model);
                }
                else
                {
                    // get entity
                    var _entity = new CONDICIONFISICA_ENFERMEDADES();
                    _entity = await _context.CONDICIONFISICA_ENFERMEDADES.FindAsync(_model.ID);
                    
                        _entity.NOMBRECIENTIFICO = _model.NOMBRECIENTIFICO;
                        _entity.NOMBRESINOMIMO = _model.NOMBRESINOMIMO;
                        _entity.DESCRIPCION = _model.DESCRIPCION;
                    

                    // actualizar registro
                        _context.Entry(_entity).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                    // redireccionar al índice
                    return RedirectToAction("Enfermedades", "Configuraciones");

                }
            }
            // to view
            return View(_model);
        }
        #endregion

        // acciones para manteniemnto de escolaridad  --YA -- sp map -- view created -- modif y probada
        #region Acciones de mant. para Escolaridades
        // todoad las escolaridades
        public async Task<ActionResult> Escolaridades()
        {
            // get entity
            var _entity = await _context.INFORMACIONACADEMICA_ESCOLARIDAD.ToListAsync();

            // get Model
            List<INFORMACIONACADEMICA_ESCOLARIDADMODEL> _model = new List<INFORMACIONACADEMICA_ESCOLARIDADMODEL>();

            // set model
            foreach (INFORMACIONACADEMICA_ESCOLARIDAD entity in _entity)
            {
                _model.Add(new INFORMACIONACADEMICA_ESCOLARIDADMODEL
                {
                    ID = entity.ID,
                    DESCRIPCIONESCOLARIDAD = entity.DESCRIPCIONESCOLARIDAD,
                    NOMBREESCOLARIDAD = entity.NOMBREESCOLARIDAD

                });

            }

            // to view
            return View(_model);
        }

        // detalle escolaridad
        public async Task<ActionResult> DetalleEscolaridad(int? ID)
        {
            // bad request si el parámetro pasado es nulo
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // get entity
            var _entity = await _context.INFORMACIONACADEMICA_ESCOLARIDAD.FindAsync(ID);

            // si no existe el objeto a consultar
            // entonces no encontrado
            if (_entity == null)
            {
                return HttpNotFound();
            }

            // get model and set model
            var _model = new INFORMACIONACADEMICA_ESCOLARIDADMODEL
            {
                ID = _entity.ID,
                DESCRIPCIONESCOLARIDAD = _entity.DESCRIPCIONESCOLARIDAD,
                NOMBREESCOLARIDAD = _entity.NOMBREESCOLARIDAD
            };

            // to view
            return View(_model);
        }


        // Nueva escolaridad
        //GET
        public ActionResult NuevaEscolaridad()
        {
            // get model
            var _model = new INFORMACIONACADEMICA_ESCOLARIDADMODEL();

            // model to view
            return View(_model);
        }

        //POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NuevaEscolaridad(INFORMACIONACADEMICA_ESCOLARIDADMODEL _model)
        {
            if (ModelState.IsValid)
            {
                // comprobar que el nombre de la escolaridad no existe
                if (_context.INFORMACIONACADEMICA_ESCOLARIDAD.FirstOrDefault(e => e.NOMBREESCOLARIDAD == _model.NOMBREESCOLARIDAD) != null)
                {
                    ModelState.AddModelError("", "Ya existe un registro con este nombre, por favor elija otro e intente de nuevo");
                    return View(_model);
                }
                else
                {

                    try
                    {

                        // set entity
                        var _entity = new INFORMACIONACADEMICA_ESCOLARIDAD
                        {
                            NOMBREESCOLARIDAD = _model.NOMBREESCOLARIDAD,
                            DESCRIPCIONESCOLARIDAD = _model.DESCRIPCIONESCOLARIDAD
                        };


                        //guardar el registro
                        _context.INFORMACIONACADEMICA_ESCOLARIDAD.Add(_entity);
                        await _context.SaveChangesAsync();

                        // redireccionar 
                        return RedirectToAction("Escolaridades", "Configuraciones");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
                    }
                }
            }

            return View(_model);
        }

        // edición de Escolaridad
        // GET
        public async Task<ActionResult> EditarEscolaridad(int? ID)
        {
            // bad request si el parámetro pasado es nulo
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // get entity
            var _entity = await _context.INFORMACIONACADEMICA_ESCOLARIDAD.FindAsync(ID);

            // si no existe el objeto a consultar
            // entonces no encontrado
            if (_entity == null)
            {
                return HttpNotFound();
            }

            // get model and set model
            var _model = new INFORMACIONACADEMICA_ESCOLARIDADMODEL
            {
                ID = _entity.ID,
                DESCRIPCIONESCOLARIDAD = _entity.DESCRIPCIONESCOLARIDAD,
                NOMBREESCOLARIDAD = _entity.NOMBREESCOLARIDAD
            };

            // to view
            return View(_model);
        }


        //POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditarEscolaridad(INFORMACIONACADEMICA_ESCOLARIDADMODEL _model)
        {
            if (ModelState.IsValid)
            {
                // comprobar que el nombre de la escolaridad no existe
                if (_context.INFORMACIONACADEMICA_ESCOLARIDAD.FirstOrDefault(e => e.NOMBREESCOLARIDAD == _model.NOMBREESCOLARIDAD && e.ID != _model.ID) != null)
                {
                    ModelState.AddModelError("", "Ya existe un registro con este nombre, por favor elija otro e intente de nuevo");
                    return View(_model);
                }
                else
                {

                    try
                    {

                        // set entity
                        var _entity = new INFORMACIONACADEMICA_ESCOLARIDAD();
                        _entity = await _context.INFORMACIONACADEMICA_ESCOLARIDAD.FindAsync(_model.ID);

                        _entity.NOMBREESCOLARIDAD = _model.NOMBREESCOLARIDAD;
                        _entity.DESCRIPCIONESCOLARIDAD = _model.DESCRIPCIONESCOLARIDAD;

                        //actualizar el registro
                        _context.Entry(_entity).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                        // redireccionar 
                        return RedirectToAction("Escolaridades", "Configuraciones");
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

        //aciones para mant,  de oficios --YA -- sp map ---- view created -- modif y probada
        #region Acciones para mant. de Oficios

        // todos los oficios 
        public async Task<ActionResult> Oficios()
        {
            // get entity
            var _entity = await _context.INFORMACIONACADEMICA_OFICIOS.ToListAsync();
            
            //get model
            List<INFORMACIONACADEMICA_OFICIOSMODEL> _model = new List<INFORMACIONACADEMICA_OFICIOSMODEL>();

            // set model
            foreach(INFORMACIONACADEMICA_OFICIOS entity in _entity)
            {
                _model.Add(new INFORMACIONACADEMICA_OFICIOSMODEL { 
                    ID = entity.ID,
                    NOMBREOFICIO = entity.NOMBREOFICIO,
                    DESCRIPCIONOFICIO = entity.DESCRIPCIONOFICIO
                });
            }

            // to view
            return View(_model);
        }

        // detalle oficio
        public async Task<ActionResult> DetalleOficio(int? ID)
        {
            // bad request, por parámetro 0 o null
            if(ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // get entity
            var _entity = await _context.INFORMACIONACADEMICA_OFICIOS.FindAsync(ID);

            //comprobar la existencia de la petición por restulato no null
            if(_entity == null)
            {
                return HttpNotFound();
            }

            // SET model
            var _model = new INFORMACIONACADEMICA_OFICIOSMODEL { 
                ID = _entity.ID,
                NOMBREOFICIO = _entity.NOMBREOFICIO,
                DESCRIPCIONOFICIO = _entity.DESCRIPCIONOFICIO

            };

            //to view
            return View(_model);
        }

        // nuevo Oficio
        // GET
        public ActionResult NuevoOficio()
        {
            // get model
            var _model = new INFORMACIONACADEMICA_OFICIOSMODEL();

            //to view
            return View(_model);
        }

        // POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]        
        public async Task<ActionResult> NuevoOficio(INFORMACIONACADEMICA_OFICIOSMODEL _model)
        {
            if(ModelState.IsValid)
            {
                // validar que ni existe el nombre
                if(_context.INFORMACIONACADEMICA_OFICIOS.FirstOrDefault(o=>o.NOMBREOFICIO == _model.NOMBREOFICIO)!= null)
                {
                    ModelState.AddModelError("","Este nombre ya ha sido asignado a otro registro, por favor ingreso otro y vuelva a intentar");
                    return View(_model);
                }else{
                    try { 
                        // set entity
                        var _entity = new INFORMACIONACADEMICA_OFICIOS { 
                            NOMBREOFICIO = _model.NOMBREOFICIO,
                            DESCRIPCIONOFICIO = _model.DESCRIPCIONOFICIO

                        };

                        // se guarda el registro
                        _context.INFORMACIONACADEMICA_OFICIOS.Add(_entity);
                       await _context.SaveChangesAsync();

                        // se reedirecciona
                        return RedirectToAction("Oficios", "Configuraciones");
                    }catch(Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
                    }
                }
            }
            return View(_model);
        }

        // edición de oficio
        // GET
        public async Task<ActionResult> EditarOficio(int? ID)
        {
            // bad request, por parámetro 0 o null
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // get entity
            var _entity = await _context.INFORMACIONACADEMICA_OFICIOS.FindAsync(ID);

            //comprobar la existencia de la petición por restulato no null
            if (_entity == null)
            {
                return HttpNotFound();
            }

            // SET model
            var _model = new INFORMACIONACADEMICA_OFICIOSMODEL
            {
                ID = _entity.ID,
                NOMBREOFICIO = _entity.NOMBREOFICIO,
                DESCRIPCIONOFICIO = _entity.DESCRIPCIONOFICIO

            };

            //to view
            return View(_model);
        }

        

        // POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditarOficio(INFORMACIONACADEMICA_OFICIOSMODEL _model)
        {
            if (ModelState.IsValid)
            {
                // validar que ni existe el nombre
                if (_context.INFORMACIONACADEMICA_OFICIOS.FirstOrDefault(o => o.NOMBREOFICIO == _model.NOMBREOFICIO && o.ID != _model.ID) != null)
                {
                    ModelState.AddModelError("", "Este nombre ya ha sido asignado a otro registro, por favor ingreso otro y vuelva a intentar");
                    return View(_model);
                }
                else
                {
                    try
                    {
                        // set entity
                        var _entity = new INFORMACIONACADEMICA_OFICIOS();
                        _entity = await _context.INFORMACIONACADEMICA_OFICIOS.FindAsync(_model.ID);

                            _entity.NOMBREOFICIO = _model.NOMBREOFICIO;
                            _entity.DESCRIPCIONOFICIO = _model.DESCRIPCIONOFICIO;

                       

                        // se actualiza el registro
                        _context.Entry(_entity).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                        // se reedirecciona
                        return RedirectToAction("Oficios", "Configuraciones");
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

        //******************************************************************************************************************
        ///---------------****************SSECCIÓN DE MANTENIMIENTO DE REHABILITACIÓN ***************-----------------------
        ///*****************************************************************************************************************
        ///
        #region mantenimiento de sección PROMOCIÓN REHABILITACIÓN
        // lista de fases
        public async Task<ActionResult> Fases()
        {
            //get entity 
            var _entity = await _context.PRO_FASE.ToListAsync();

            // set model
            List<PRO_FASEMODEL> _model = new List<PRO_FASEMODEL>();

            //model set
            foreach(PRO_FASE entity in _entity)
            {
                _model.Add(new PRO_FASEMODEL { 
                    ID = entity.ID,
                    NOMBRE = entity.NOMBRE,
                    DESCRIPCION = entity.DESCRIPCION
                });
            }

            // to view
            return View(_model);
        }

        // detalle fase

        public async Task<ActionResult> DetalleFase(int? ID)
        {
            // comprobar la nulidad de ID
            if(ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

 
            //get entity
            var _entity = await _context.PRO_FASE.FindAsync(ID);

            // comprobar si existe un registro con es id
            if(_entity == null)
            {
                return HttpNotFound();
            }

            // set model
            var _model = new PRO_FASEMODEL {
                ID = _entity.ID,
                NOMBRE = _entity.NOMBRE,
                DESCRIPCION = _entity.DESCRIPCION
            };

            // to view
            return View(_model);
        }
        
        // crear fase
        
        //get 
        public ActionResult NuevaFase()
        {
            // get model 
            var _model = new PRO_FASEMODEL();

            return View(_model);
        }

        //post
        public async Task<ActionResult> NuevaFase(PRO_FASEMODEL _model)
        {
            if(ModelState.IsValid)
            {

                try
                {
                    // coprobar la existencia del nombre
                    if (_context.PRO_FASE.FirstOrDefault(i => i.NOMBRE == _model.NOMBRE) != null)
                    {
                        ModelState.AddModelError("", "Ya existe un registro con este nombre, por favor elija otro!");
                        return View(_model);
                    }
                    else {
                        // get entity
                        var _entity = new PRO_FASE
                        {
                            NOMBRE = _model.NOMBRE,
                            DESCRIPCION = _model.DESCRIPCION
                        };

                        // guardar en el context
                        _context.PRO_FASE.Add(_entity);
                        await _context.SaveChangesAsync();

                        // to lista de Fases
                        return RedirectToAction("Fases","Configuraciones");
                    }

                    
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(_model);
                }
            }

            return View(_model);
        }

        //editar fase 
        // get
        public async Task<ActionResult> EditarFase(int? ID)
        {
            // comprobar la nulidad de ID
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            //get entity
            var _entity = await _context.PRO_FASE.FindAsync(ID);

            // comprobar si existe un registro con es id
            if (_entity == null)
            {
                return HttpNotFound();
            }

            // set model
            var _model = new PRO_FASEMODEL
            {
                ID = _entity.ID,
                NOMBRE = _entity.NOMBRE,
                DESCRIPCION = _entity.DESCRIPCION
            };

            // to view
            return View(_model);
        }

        //post
        public async Task<ActionResult> EditarFase(PRO_FASEMODEL _model)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    // coprobar la existencia del nombre
                    var _test = await _context.PRO_FASE.FirstOrDefaultAsync(i=>i.NOMBRE == _model.NOMBRE);
                    bool _nombreRepetido = false;
                    _nombreRepetido = (_test.ID != _model.ID && _test.NOMBRE == _model.NOMBRE) ? true : false;

                    if (_nombreRepetido)
                    {
                        ModelState.AddModelError("", "Ya existe un registro con este nombre, por favor elija otro!");
                        return View(_model);
                    }
                    else
                    {
                        // get entity
                        var _entity = new PRO_FASE();
                        _entity = await _context.PRO_FASE.FindAsync(_model.ID);

                        _entity.NOMBRE = _model.NOMBRE;
                        _entity.DESCRIPCION = _model.DESCRIPCION;
                        

                        // guardar en el context
                        _context.Entry(_entity).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                        // to lista de Fases
                        return RedirectToAction("Fases", "Configuraciones");
                    }


                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(_model);
                }
            }

            return View(_model);
        }
        
        #endregion

        // dispose data base context
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                _context.Dispose(); 
            }
            base.Dispose(disposing);
        }
    }
}