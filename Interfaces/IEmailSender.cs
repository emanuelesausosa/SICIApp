using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SICIApp.Entities;

namespace SICIApp.Interfaces
{
    public interface IEmailSender
    {
        EnviarEmailCreateStatus EnviarEmail(string para, string asunto, string cuerpo);
    }
}
