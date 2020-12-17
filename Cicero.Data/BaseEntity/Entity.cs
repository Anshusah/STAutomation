using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Data.BaseEntity
{
    public abstract class Entity<TKey>
    {
        public TKey Id { get; set; }
    }
}
