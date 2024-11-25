using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace backend.Services
{
    public class EmailService
    {

        //Configuration variables for smtp server 
        private readonly static string smtpAddress = "smtp.gmail.com";
        private readonly static int portNumber = 587;
        private readonly static bool enableSSL = true;

        //Configuration variables for email account
        private readonly static string serverEmailAddress = "educonnect.mail.bot@gmail.com";
        private readonly static string serverEmailAppPassword = "zhcy zdja bloj kqaj";

        //Method for sending email to given recipient with the given parameter subject and body
        public async static Task<bool> SendEmailToAsync(string sendToEmailAddress, string subject, string body, bool isBodyHtml = false)
        {

            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(serverEmailAddress);
                    mail.To.Add(sendToEmailAddress);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = isBodyHtml;

                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                    {
                        smtp.Credentials = new NetworkCredential(serverEmailAddress, serverEmailAppPassword);
                        smtp.EnableSsl = enableSSL;
                        await smtp.SendMailAsync(mail);
                    }
                }
                Console.WriteLine("LOG::EmailService::SendEmailToAsync::Email sent successfully to " + sendToEmailAddress);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("LOG::EmailService::SendEmailToAsync::Error sending email to " + sendToEmailAddress + " with error: " + ex.Message);
                return false;
            }

        }
    }
}