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
        public async Task<ActionResult> Solicitudes()
        {
            return View();
        } 

        // Nueva Solicitud
        public ActionResult NuevaSolicitud()
        {
            return View();
        }
        #endregion

    }
}