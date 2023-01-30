using CSBlog.Dtos.Email;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace CSBlog.Services;

public class EmailService
{
    private IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public void SendEmail(SendEmailReq body)
    {
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
        message.To.Add(MailboxAddress.Parse(body.Email));
        message.Subject = body.Subject;
        message.Body = new TextPart(TextFormat.Html) { Text = body.Content };
        using var client = new SmtpClient();
        client.Connect(
            _config.GetSection("EmailHost").Value,
            int.Parse(_config.GetSection("EmailPort").Value),
            MailKit.Security.SecureSocketOptions.StartTls
        );
        client.Authenticate(
            _config.GetSection("EmailUsername").Value,
            _config.GetSection("EmailPassword").Value
        );
        client.Send(message);
        client.Disconnect(true);
    }
}
