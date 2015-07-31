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

        //Acciones de Solicitudes -- flow 1
        #region Acciones de Solicitudes, Flow 1 - primer paso
        //public async Task<ActionResult> Solicitudes()
        //{
        //    return View();
        //} 

        // Nueva Solicitud
        public ActionResult NuevaSolicitud()
        {
            return View();
        }

        //ajax actions
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> NuevaFicha(FICHAMODEL _model)
        {
            //simulación conexión lenta
            Thread.Sleep(2000);

            string error = "";
            if(_context.FICHA.FirstOrDefault(f=>f.NUMEROIDENTIDAD == _model.NUMEROIDENTIDAD) != null)
            {
                // set entity
                var _entity = new FICHA { 
                     NUMEROIDENTIDAD = _model.NUMEROIDENTIDAD,
                     NUMEROPASAPORTE = _model.NUMEROPASAPORTE,
                     PRIMERNOMBRE = _model.PRIMERAPELLIDO,
                     SEGUNDONOMBRE = _model.SEGUNDONOMBRE,
                     PRIMERAPELLIDO = _model.PRIMERAPELLIDO,
                     SEGUNDOAPELLIDO = _model.SEGUNDOAPELLIDO,
                     NACIONALIDAD = _model.NACIONALIDAD
                };

                // guardar el registro
                _context.FICHA.Add(_entity);
                await _context.SaveChangesAsync();

                return Json("'Success':'true'", JsonRequestBehavior.AllowGet);
            }

            return Json(String.Format("'Success':'false', 'Error':'{0}'"),error,  JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}