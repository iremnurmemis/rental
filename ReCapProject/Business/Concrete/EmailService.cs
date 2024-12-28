
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Business
{
    public class EmailService:IEmailHelper
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(string ToEmail,string subject,string body)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings");
            var smtpClient = new SmtpClient(smtpSettings["Server"])
            {
                Port = int.Parse(smtpSettings["Port"]),
                Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]),
                EnableSsl = true, // Bu satır güvenli bağlantı için gereklidir
                UseDefaultCredentials=false
            };


            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpSettings["Username"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(ToEmail);

            smtpClient.Send(mailMessage);
        }


    }

}
