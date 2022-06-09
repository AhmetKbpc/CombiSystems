using CombiSystems.Business.Repositories.Abstracts.EntityFrameworkCore;
using CombiSystems.Core.Entities;
using CombiSystems.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombiSystems.Business.Repositories;

public class AppointmentRepo : RepositoryBase<Appointment, int>
{
    public AppointmentRepo(MyContext context) : base(context)
    {
    }
}
