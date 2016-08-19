using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TravelService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TravelService" in both code and config file together.
    public class TravelService : ITravelService
    {
        public WebServiceProviderResponse synchronize(WebServiceConsumerRequest webServiceConsumerRequest)
        {
            throw new NotImplementedException();
        }
    }
}
