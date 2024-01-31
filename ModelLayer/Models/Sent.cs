using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Models
{
    public class Sent
    {
        public string SendingMail(string emailTo)
        {
            try
            {
                string emailFrom = "aishwaryagk2021@gmail.com";
                string from_pass = "hgvq pygg wqqp wkrq";
                MailMessage message = new MailMessage(emailFrom, emailTo);
                // string mailBody = "Token generated:  ";
                message.Subject = "Forgot Password";
                message.Body = "click here to reset your password";
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = false;

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;

                smtpClient.Credentials = new NetworkCredential(emailFrom, from_pass);
                smtpClient.Send(message);
                return "Password reset send to email" + emailFrom;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

