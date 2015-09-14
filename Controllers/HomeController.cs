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
    public class HomeController : Controller
    {
        // presets de inicio
        #region Presets de inicio
        public IEmailSender _emailsender { get; set; }
        public SICIBD2Entities1 _context { get; set; }
        public IDBFactory _dbfactory { get; set; }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            if (this._emailsender == null || _context == null || _dbfactory == null)
            {
                //     this._repository = new DBRepository();
                this._context = new SICIBD2Entities1();
                this._emailsender = new EmailSender();
                this._dbfactory = new DBFactory();
            }
            base.Initialize(requestContext);
        } 
        #endregion

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        // elementos JSON dash boards
        // se obtienen de dbFactory
        [Authorize]
        #region Graficos para dashboards
        public ActionResult GetTopCasosUsoDrogas()
        {
            var _topdrogas = _dbfactory.GetTopDrogas;

            return Json(_topdrogas, JsonRequestBehavior.AllowGet);
        }
        #endregion


        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}