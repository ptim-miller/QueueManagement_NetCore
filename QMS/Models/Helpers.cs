using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace QMS.Models
{
    internal static class Helpers
    {
        internal static string getParam(string param)
        {
            IConfigurationRoot Configuration;
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            return Configuration["Params:" + param];
        }

        internal static async Task<Boolean> SendMessage(string to, string message)
        {
            try
            {
                string mailServer = Helpers.getParam("HostServer");
                string mailServerPort = Helpers.getParam("HostServerPort");
                string ssl = Helpers.getParam("HostServerEnableSSL");
                bool mailServerSSL = false;
                if (ssl == "true" || ssl == "True")
                {
                    mailServerSSL = true;
                }
                string mailServerLogin = Helpers.getParam("HostServerLogin");
                string mailServerCredential = Helpers.getParam("HostServerCredential");
                string EmailFrom = mailServerLogin;
                string EmailTo = to;
                SmtpClient client = new SmtpClient(mailServer);
                if (mailServerPort != null)
                {
                    int num;
                    var parsed = Int32.TryParse(mailServerPort, out num);
                    if (parsed)
                        client.Port = num;
                }
                client.EnableSsl = mailServerSSL;
                using (MailMessage bodyMessage = new MailMessage(EmailFrom, EmailTo))
                {
                    if (mailServerLogin != null)
                    {
                        var credential = new NetworkCredential()
                        {
                            UserName = mailServerLogin,
                            Password = mailServerCredential
                        };
                        client.Credentials = credential;
                    }
                    bodyMessage.IsBodyHtml = true;
                    bodyMessage.Body = message;
                    bodyMessage.BodyEncoding = System.Text.Encoding.UTF8;
                    bodyMessage.Subject = "Notification";
                    bodyMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                    client.Timeout = 7000;
                    var status = await client.SendMailAsync(bodyMessage).ContinueWith(x => x.Status);
                    if (status == TaskStatus.RanToCompletion)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        internal static Boolean NotifyAdmin(string message)
        {
            try
            {
                string mailServer = Helpers.getParam("HostServer");
                string mailServerPort = Helpers.getParam("HostServerPort");
                string ssl = Helpers.getParam("HostServerEnableSSL");
                bool mailServerSSL = false;
                if (ssl == "true" || ssl == "True")
                {
                    mailServerSSL = true;
                }
                string mailServerLogin = Helpers.getParam("HostServerLogin");
                string mailServerCredential = Helpers.getParam("HostServerCredential");
                string EmailFrom = mailServerLogin;
                string EmailTo = Helpers.getParam("WebAdmin");
                SmtpClient client = new SmtpClient(mailServer);
                if (mailServerPort != null)
                {
                    int num;
                    var parsed = Int32.TryParse(mailServerPort, out num);
                    if (parsed)
                        client.Port = num;
                }
                client.EnableSsl = mailServerSSL;
                using (MailMessage bodyMessage = new MailMessage(EmailFrom, EmailTo))
                {
                    if (mailServerLogin != null)
                    {
                        var credential = new NetworkCredential()
                        {
                            UserName = mailServerLogin,
                            Password = mailServerCredential
                        };
                        client.Credentials = credential;
                    }
                    bodyMessage.IsBodyHtml = true;
                    bodyMessage.Body = message;
                    bodyMessage.BodyEncoding = System.Text.Encoding.UTF8;
                    bodyMessage.Subject = "QMS - System Error";
                    bodyMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                    client.Timeout = 7000;
                    var status = client.SendMailAsync(bodyMessage).ContinueWith(x => x.Status).Result;
                    if (status == TaskStatus.RanToCompletion)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

    }
}
