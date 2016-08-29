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

                Int32 ServiceID = StoredProceduresCall.InsertService(SessionID: SessionID, Status:Convert.ToInt32(ServiceStatus.Initial));
                WebServiceProviderResponse WPR = new WebServiceProviderResponse();

                if (webServiceConsumerRequest.SendBookingRequirementRequests != null) 
                        WPR.SendBookingRequirementResponses = SendBookingRequirementResponses(webServiceConsumerRequest, ServiceID);
                if (webServiceConsumerRequest.CancelBookingRequirementRequests != null)
                    WPR.CancelBookingRequirementResponses = CancelBookingRequirementResponses(webServiceConsumerRequest);





                    return WPR;
            }// if (webServiceConsumerRequest != null)
            else
                return null;

        }// public WebServiceProviderResponse synchronize(WebServiceConsumerRequest webServiceConsumerRequest)


        public RequirementResponse[] SendBookingRequirementResponses(WebServiceConsumerRequest webServiceConsumerRequest, Int32 ServiceID)
        {

            RequirementResponse[] rrf = null;
            List<RequirementResponse> list = new List<RequirementResponse>();
            SendBookingRequirementRequest[] SendBookingRequirementRequests = webServiceConsumerRequest.SendBookingRequirementRequests;


            if (SendBookingRequirementRequests == null)
                return null;
                     
            foreach (SendBookingRequirementRequest sbrr in SendBookingRequirementRequests)
            {
                StoredProceduresCall.insert_update_SendBookingRequirementRequests(sbrr, ServiceID);
                RequirementResponse rr = new RequirementResponse();
                long BookingRequirementId = sbrr.BookingRequirementId;
                if (BookingRequirementId != 0)
                {
                    rr.BookingRequirementId = BookingRequirementId;
                    rr.Comment = "";
                    rr.IsReceived = true;
                    StoredProceduresCall.insert_RequirementResponse(rr, Convert.ToInt32(RequirementResponseStatus.Send));
                }
             
                list.Add(rr);


            }
            rrf = list.Cast<RequirementResponse>().ToArray();
            // wpr.r
            return rrf;

        }// public RequirementResponse[] SendBookingRequirementResponses(WebServiceConsumerRequest webServiceConsumerRequest, Int32 ServiceID)

        public RequirementResponse[] CancelBookingRequirementResponses(WebServiceConsumerRequest webServiceConsumerRequest) {

            RequirementResponse[] rrf = null;
            List<RequirementResponse> list = new List<RequirementResponse>();
            CancelBookingRequirementRequest[] cbrrField = webServiceConsumerRequest.CancelBookingRequirementRequests;

            if (cbrrField == null)
                return null;

            foreach (CancelBookingRequirementRequest cbrr in cbrrField) {

                StoredProceduresCall.insert_update_CancelBookingRequirementRequest(cbrr);
                RequirementResponse rr = new RequirementResponse();
                long BookingRequirementId = cbrr.BookingRequirementId;
                if (BookingRequirementId != 0)
                {

                    rr.BookingRequirementId = BookingRequirementId;
                    rr.Comment = cbrr.Comment;
                    rr.IsReceived = true;
                    StoredProceduresCall.insert_RequirementResponse(rr, Convert.ToInt32(RequirementResponseStatus.Cancel));
                }
                list.Add(rr);
            }


            rrf = list.Cast<RequirementResponse>().ToArray();
            // wpr.r
            return rrf;
        }//public RequirementResponse[] CancelBookingRequirementResponses(WebServiceConsumerRequest webServiceConsumerRequest) {

        public BookingResponse[] AcceptBookingResponses(WebServiceConsumerRequest webServiceConsumerRequest, Int32 ServiceID) {

            BookingResponse[] brfield = null;
            List<BookingResponse> list = new List<BookingResponse>();
            AcceptBookingRequest[] abrField = webServiceConsumerRequest.AcceptBookingRequests;

            if (abrField == null)
                return null;

            foreach (AcceptBookingRequest abr in abrField) {

                StoredProceduresCall.insert_update_AcceptBookingRequest(abr, ServiceID);
                BookingResponse br = new BookingResponse();
                long BookingID = abr.BookingId;
                if (BookingID != 0)
                {

                    br.BookingId = BookingID;
                    br.Comment = "";
                    br.IsReceived = true;

                    StoredProceduresCall.insert_BookingResponse(br, ServiceID);
                }
                list.Add(br);
            }

            brfield= list.Cast<BookingResponse>().ToArray();
            return brfield;
        }//public BookingResponse[] AcceptBookingResponses(WebServiceConsumerRequest webServiceConsumerRequest, Int32 ServiceID) {

    }

}
