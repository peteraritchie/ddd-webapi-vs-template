using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public Guid Id { get; }

        public EntityNotFoundException(Guid id)
        {
            Id = id;
        }
    }
}
