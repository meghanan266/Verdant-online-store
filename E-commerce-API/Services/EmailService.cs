using E_commerce_API.DataModel;
using MailKit.Net.Smtp;
using MimeKit;

namespace E_commerce_API.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;
        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void SendEmail(EmailDTO emailDto)
        {
            var emailMsg = new MimeMessage();
            var from = configuration["EmailSettings:From"];
            emailMsg.From.Add(new MailboxAddress("Verdant - Close To Nature", from));
            emailMsg.To.Add(new MailboxAddress(emailDto.To, emailDto.To));
            emailMsg.Subject = emailDto.Subject;
            emailMsg.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = string.Format(emailDto.Content)
            };
            using(var client = new SmtpClient())
            {
                try
                {
                    client.Connect(configuration["EmailSettings:SmtpServer"], 465, true);
                    client.Authenticate(configuration["EmailSettings:From"], configuration["EmailSettings:Password"]);
                    client.Send(emailMsg);
                }
                catch(Exception ex)
                {
                    throw;
                }
                finally
                {
                    client?.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}
