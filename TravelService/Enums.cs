using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelService
{
    enum RequirementResponseStatus : int {

        Send=1,
        Cancel
    }
    enum ServiceStatus : int {

        Initial=1,
        Viewed,
        TA
    }
}
