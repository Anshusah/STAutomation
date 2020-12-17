using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Cicero.Service.Helpers
{
    public class EmailSender: IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(message);

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("vesuviois.info@gmail.com");
                mail.To.Add(email);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                //Attachment attachment = null;
                
                //foreach (var item in images)
                //{
                //    attachment = new Attachment(item);
                //}
                //mail.Attachments.Add(attachment);

                mail.Body = sb.ToString();

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("vesuviois.info@gmail.com", "vesuviois@123");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

                await Task.CompletedTask;

            }
            catch (Exception)
            {
            }
        }

        public async Task SendEmailAttachmentAsync(string email, string subject, string message, List<string> images = null)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(message);

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("vesuviois.info@gmail.com");
                mail.To.Add(email);
                mail.Subject = subject;
                mail.IsBodyHtml = true;

                if (images.Count > 0)
                {
                    Attachment attachment = null;
                    foreach (var item in images)
                    {
                        attachment = new Attachment(item);
                        mail.Attachments.Add(attachment);
                    }

                }

                mail.Body = sb.ToString();

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("vesuviois.info@gmail.com", "vesuviois@123");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

                await Task.CompletedTask;

            }
            catch (Exception)
            {
            }
        }
    }
}
