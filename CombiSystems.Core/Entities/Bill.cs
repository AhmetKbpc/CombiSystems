using CombiSystems.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombiSystems.Core.Entities;

public class Bill : BaseEntity<int>
{
    public decimal TotalPrice { get; set; }
    public bool PaymentStatus { get; set; } = false;

    public int AppointmentId { get; set; }
    
    public Appointment? Appointment { get; set; }
    public IList<BillDetails>? BillDetails { get; set; }


}
