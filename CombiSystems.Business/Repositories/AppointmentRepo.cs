using CombiSystems.Business.Repositories.Abstracts.EntityFrameworkCore;
using CombiSystems.Core.Entities;
using CombiSystems.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CombiSystems.Business.Repositories;

public class AppointmentRepo : RepositoryBase<Appointment, int>
{
    public AppointmentRepo(MyContext context) : base(context)
    {
    }
    public override IQueryable<Appointment> Get(Expression<Func<Appointment, bool>> predicate = null)
    {
        return predicate == null ? _table.OrderBy(x => x.Id) : _table.Where(predicate).OrderBy(x => x.Id);
    }
}
