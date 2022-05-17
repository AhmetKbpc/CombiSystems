using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombiSystems.Core.Emails;

public class MailModel
{
    public List<EmailModel> To { get; set; } = new();
    public List<EmailModel> Cc { get; set; } = new();
    public List<EmailModel> Bcc { get; set; } = new();
    public string Subject { get; set; }
    public string Body { get; set; }
    public List<Stream> Attachs { get; set; }
}

