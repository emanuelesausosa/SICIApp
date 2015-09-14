using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SICIApp.ViewModels
{
    // en esta seccipon se inicializan los modelos para presentación de datos en el dashboard

    //esta view model modela el dashboard de top de drogas y sus casos de occuerencia
    public class TopDrogasViewModel
    {
        public Int64 RANK { get; set; }
        public int OCURRENCIA { get; set; }
        public string NOMBRECIENTIFICO { get; set; }
    }
}