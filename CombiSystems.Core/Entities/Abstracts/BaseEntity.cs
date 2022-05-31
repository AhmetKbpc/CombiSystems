using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombiSystems.Core.Entities.Abstracts;

public abstract class BaseEntity<T> where T : IEquatable<T>
{
    public T Id { get; set; }
    public string CreatedUser { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? UpdatedUser { get; set; }
}
