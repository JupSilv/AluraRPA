using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AluraRPA.Domain.Entities
{
    public record ResultProcess(bool sucess, string status, object obs);
}
