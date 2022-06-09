using CombiSystems.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombiSystems.Core.Entities;

public class Appointment : BaseEntity<int>
{
    public string? UserId { get; set; }

    public DateTime AppointentOpeningDate { get; set; } = DateTime.UtcNow;
    public DateTime TechnicianAssignDate { get; set; }
    public DateTime AppointentClosingDate { get; set; }

    public string? Description { get; set; }

    public string? AppointmentAddress { get; set; }

    public bool TaskStatus { get; set; }=false;

    public string? TechnicianId { get; set; }

    public Bill? Bill { get; set; }


}

