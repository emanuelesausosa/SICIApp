using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using SICIApp.Interfaces;
using SICIApp.Entities;


namespace SICIApp.Dominio
{
    public class EmailSender : IEmailSender
    {
        public const string clienteMail = "emanuelesausosa@gmail.com";
        public const string clientePassword = "*******************";
        public const string smtpCliente = "smtp.gmail.com";
        public const int smtpPuerto = 587;
        public const bool enableSsl = true;

        public EnviarEmailCreateStatus EnviarEmail(string para, string asunto, string cuerpo)
        {
            try
            {

                MailMessage mail = new MailMessage();
                SmtpClient smtp = new SmtpClient(smtpCliente);
                smtp.Credentials = new NetworkCredential(clienteMail, clientePassword);
                smtp.Port = smtpPuerto;
                smtp.EnableSsl = enableSsl;

                mail.From = new MailAddress(clienteMail);

                mail.To.Add(para);
                mail.CC.Add(clienteMail);
                mail.Subject = asunto;
                mail.Body = cuerpo;

                smtp.Send(mail);

                return EnviarEmailCreateStatus.Exito;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return EnviarEmailCreateStatus.ErrorGeneral;
            }
        }
    }
}
