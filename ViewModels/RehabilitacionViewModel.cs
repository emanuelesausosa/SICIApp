using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Web.Mvc;
using SICIApp.Dominio;
using SICIApp.Entities;

namespace SICIApp.ViewModels
{
    public class RehabilitacionViewModel
    {
        public RehabilitacionViewModel()
        {
            this.Fases = new HashSet<PRO_FASE>();
            this.Niveles = new HashSet<PRO_NIVEL>();
            this.PromocionesNiveles = new HashSet<PRO_PROMOCIONNIVEL>();
        }

        public ICollection<PRO_FASE> Fases { get; set; }
        public ICollection<PRO_NIVEL> Niveles { get; set; }
        public ICollection<PRO_PROMOCIONNIVEL> PromocionesNiveles { get; set; }
    }
}