using System.Net;
using System.Net.Mail;
using System.Text;
using CombiSystems.Core.Configurations;
using CombiSystems.Core.Emails;
using Microsoft.Extensions.Configuration;


namespace CombiSystems.Business.Services.Email;

public class SmtpEmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailSettings EmailSettings { get; }

    public SmtpEmailService(IConfiguration configuration)
    {
        _configuration = configuration;
        var emailsettings = _configuration.GetSection("GmailSettings");
        this.EmailSettings = new EmailSettings
        {
            SenderMail = emailsettings["SenderMail"],
            Password = emailsettings["Password"],
            Smtp = emailsettings["Smtp"],
            SmtpPort = Convert.ToInt32(emailsettings["SmtpPort"])

        };
    }
    public EmailSettings emailSettings { get; }

    public Task SendMailAsync(MailModel model)
    {
        var mail = new MailMessage { From = new MailAddress(this.EmailSettings.SenderMail) };

        foreach (var c in model.To)
        {
            mail.To.Add(new MailAddress(c.Adress, c.Name));
        }

        foreach (var cc in model.Cc)
        {
            mail.CC.Add(new MailAddress(cc.Adress, cc.Name));
        }
        foreach (var cc in model.Bcc)
        {
            mail.Bcc.Add(new MailAddress(cc.Adress, cc.Name));
        }

        if (model.Attachs is { Count: > 0 })
        {
            foreach (var attach in model.Attachs)
            {
                var fileStream = attach as FileStream;
                var info = new FileInfo(fileStream.Name);

                mail.Attachments.Add(new Attachment(attach, info.Name));
            }
        }

        mail.IsBodyHtml = true;
        mail.BodyEncoding = Encoding.UTF8;
        mail.SubjectEncoding = Encoding.UTF8;
        mail.HeadersEncoding = Encoding.UTF8;

        mail.Subject = model.Subject;
        mail.Body = model.Body;

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        var smtpClient = new SmtpClient(this.EmailSettings.Smtp, this.EmailSettings.SmtpPort)
        {
            Credentials = new NetworkCredential(this.EmailSettings.SenderMail, this.EmailSettings.Password),
            EnableSsl = true
        };
        return smtpClient.SendMailAsync(mail);
    }
}