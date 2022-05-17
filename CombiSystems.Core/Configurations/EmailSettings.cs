using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombiSystems.Core.Configurations;

public class EmailSettings
{
    public string SenderMail { get; set; }
    public string Password { get; set; }
    public string Smtp { get; set; }
    public int SmtpPort { get; set; }
}