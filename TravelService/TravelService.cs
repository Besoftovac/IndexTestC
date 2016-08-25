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
           
            if (webServiceConsumerRequest != null)
            {
                long SessionID = webServiceConsumerRequest.SessionId;

                Int32 ServiceID = StoredProceduresCall.InsertService(SessionID: SessionID);
                WebServiceProviderResponse WPR = new WebServiceProviderResponse();

                if (webServiceConsumerRequest.SendBookingRequirementRequests != null) {
                    if(ServiceID !=-1)
                        WPR.SendBookingRequirementResponses = SendBookingRequirementResponses(webServiceConsumerRequest, ServiceID);                }
                    

               // if(webServiceConsumerRequest.CancelBookingRequirementRequests != null)

                    return WPR;
            }
            else
                return null;
            
        }

       
        public RequirementResponse[] SendBookingRequirementResponses(WebServiceConsumerRequest webServiceConsumerRequest, Int32 ServiceID)
        {

            RequirementResponse[] rrf = null;
            List<RequirementResponse> list = new List<RequirementResponse>();
            SendBookingRequirementRequest[] SendBookingRequirementRequests = null;
            SendBookingRequirementRequests = webServiceConsumerRequest.SendBookingRequirementRequests;


            if (SendBookingRequirementRequests == null)
                return null;
                     
            foreach (SendBookingRequirementRequest sbrr in SendBookingRequirementRequests)
            {
                StoredProceduresCall.insert_update_SendBookingRequirementRequests(sbrr, ServiceID);
                RequirementResponse rr = new RequirementResponse();
                long BookingRequirementId = sbrr.BookingRequirementId;
                rr.BookingRequirementId = BookingRequirementId;
                rr.Comment = "";
                rr.IsReceived = true;
                list.Add(rr);

            }
            rrf = list.Cast<RequirementResponse>().ToArray();
            // wpr.r
            return rrf;

        }

    }

}
