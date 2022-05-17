
using CombiSystems.Core.Emails;

namespace CombiSystems.Business.Services.Email;

public interface IEmailService
{
    Task SendMailAsync(MailModel model);
}
