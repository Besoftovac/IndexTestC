using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Web.Script.Serialization;

namespace TravelService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TravelService" in both code and config file together.
    //[AspNetCompatibilityRequirements(RequirementsMode
    //= AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class TravelService : ITravelService
    {
        public WebServiceProviderResponse synchronize(WebServiceConsumerRequest webServiceConsumerRequest)
        {
            // throw new NotImplementedException();
            return null;
        }

        public String test(Int32 nmb)
        {

            if (nmb.Equals(null))
            {
                return "Broj nije defitniran!";
            }
            if (nmb == 1)
                return "Uneseni broj je 1!";
            else
                return "Uneseni broj nije 1!"; 

        }
    }

}
