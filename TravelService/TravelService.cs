using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;

namespace TravelService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TravelService" in both code and config file together.
    [AspNetCompatibilityRequirements(RequirementsMode
    = AspNetCompatibilityRequirementsMode.Allowed)]
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

                if (webServiceConsumerRequest.SendBookingRequirementRequests != null)
                    WPR.SendBookingRequirementResponses = SendBookingRequirementResponses(webServiceConsumerRequest, ServiceID);
                if (webServiceConsumerRequest.CancelBookingRequirementRequests != null)
                    WPR.CancelBookingRequirementResponses = CancelBookingRequirementResponses(webServiceConsumerRequest);
                if (webServiceConsumerRequest.AcceptBookingRequests != null)
                    WPR.AcceptBookingResponses = AcceptBookingResponses(webServiceConsumerRequest, ServiceID);
                if (webServiceConsumerRequest.CancelBookingRequests != null)
                    WPR.CancelBookingResponses = CancelBookingResponses(webServiceConsumerRequest, ServiceID);
                if (webServiceConsumerRequest.RequireTicketsRequests != null)
                    WPR.RequireTicketsResponses = RequireTicketsResponses(webServiceConsumerRequest, ServiceID);

                return WPR;
            }// if (webServiceConsumerRequest != null)
            else
                return null;

        }// public WebServiceProviderResponse synchronize(WebServiceConsumerRequest webServiceConsumerRequest)
        public static DateTime date(DateTime date) {

            DateTime dt;
            bool success = DateTime.TryParseExact(date.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt);
            if (success)
            {
                var result = dt.ToString("yyyy-MM-dd");
            }

            return dt;
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public PersonTA Initialize_PersonTA(SendBookingRequirementRequest sbrr) {

            //Thread.CurrentThread.CurrentCulture = new CultureInfo("hr-HR");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-CA");

            Person person = sbrr.Person;
            PersonTA personTA = new PersonTA();
            DateTime defaultDate = new DateTime(1756, 1, 1);


            personTA.FirstName = person.FirstName;
            personTA.LastName = person.LastName;
            personTA.PassportNumber = person.PassportNumber;
            personTA.PassportExpiryDate = (person.PassportExpiryDate == null || person.PassportExpiryDate.ToString() == "1.1.0001. 0:00:00") ? defaultDate : person.PassportExpiryDate;
            personTA.PassportIssuingCountry = person.PassportIssuingCountry;
            personTA.Birthday = (person.Birthday == null || person.Birthday.ToString() == "1.1.0001. 0:00:00") ? defaultDate : person.Birthday;
            personTA.PlaceOfBirth = person.PlaceOfBirth;
            personTA.Nationality = person.Nationality;
            personTA.SeamansBookNumber = person.SeamansBookNumber;
            personTA.SeamansBookExpiryDate = (person.SeamansBookExpiryDate == null || person.SeamansBookExpiryDate.ToString() == "1.1.0001. 0:00:00") ? defaultDate : person.SeamansBookExpiryDate;
            personTA.SeamansBookIssuingCountry = person.SeamansBookIssuingCountry;
            personTA.USVisaNumber = person.USVisaNumber;
            personTA.USVisaExpiryDate = (person.USVisaExpiryDate == null || person.USVisaExpiryDate.ToString() == "1.1.0001. 0:00:00") ? defaultDate : person.USVisaExpiryDate;
            personTA.PersonComment = person.PersonComment;
            personTA.Date_ = defaultDate;
            personTA.BookingRequirementIdHC = (Int32)sbrr.BookingRequirementId;

            return personTA;
        }//public PersonTA Initialize_PersonTA(SendBookingRequirementRequest sbrr)

        public SendBookingRequirementRequestTA Initialize_SBRRequestTA(SendBookingRequirementRequest sbrr, Int32 ServiceID)  {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-CA");


            SendBookingRequirementRequestTA sbrrTA = new SendBookingRequirementRequestTA();
            DateTime defaultDate = new DateTime(1756,1,1);
          
            //defaultDate.GetDateTimeFormats
            //if (sbr.DepartureDate != null && sbr.DepartureDate.ToString() != "1.1.0001. 0:00:00" )  
            sbrrTA.FromAirport = sbrr.FromAirport;
            sbrrTA.ToAirport = sbrr.ToAirport;
            sbrrTA.DepartureDate = (sbrr.DepartureDate == null || sbrr.DepartureDate.ToString() == "1.1.0001. 0:00:00") ? defaultDate : sbrr.DepartureDate;
            sbrrTA.ArrivalDate = (sbrr.ArrivalDate == null || sbrr.ArrivalDate.ToString() == "1.1.0001. 0:00:00") ? defaultDate : sbrr.ArrivalDate;
            sbrrTA.BookingRequirementIdHC = (Int32)sbrr.BookingRequirementId;
            sbrrTA.RequestBookingComment = sbrr.RequestBookingComment;
            sbrrTA.Service_Id = ServiceID;
            sbrrTA.Person_Id = 0;
            sbrrTA.Date_ = defaultDate;
            sbrrTA.Status = Convert.ToInt32(ServiceStatus.Initial);

            return sbrrTA;

        }

        public TestRequest initialize_testing(SendBookingRequirementRequest sbrr, Int32 ServiceID) {
            TestRequest tr = new TestRequest();

            tr.Comment = "Radi!";
            tr.Requ = true;
            tr.ServiceID = ServiceID;
            tr.BookingID = sbrr.BookingRequirementId;

            return tr; 
            


        }

        public void SendBookingRequirementResponses_field(WebServiceConsumerRequest webServiceConsumerRequest, Int32 ServiceID)
        {           
            List<PersonTA> listPerson = new List<PersonTA>();
            List<SendBookingRequirementRequestTA> listsbrr = new List<SendBookingRequirementRequestTA>();
            //List<TestRequest> listtr = new List<TestRequest>();

            SendBookingRequirementRequest[] sbrrfield = webServiceConsumerRequest.SendBookingRequirementRequests;

            foreach (SendBookingRequirementRequest sbrr in sbrrfield) {

                SendBookingRequirementRequestTA sbrrTA = Initialize_SBRRequestTA(sbrr, ServiceID);
                PersonTA personTA = Initialize_PersonTA(sbrr);
                //TestRequest tr = initialize_testing(sbrr, ServiceID);

                listPerson.Add(personTA);
                listsbrr.Add(sbrrTA);
                //listtr.Add(tr);
            }

            DataTable dtsbrr = ToDataTable<SendBookingRequirementRequestTA>(listsbrr);
            DataTable dtPerson = ToDataTable<PersonTA>(listPerson);
            //DataTable dttr= ToDataTable<TestRequest>(listtr);

            //if(dtPerson != null)
            //    StoredProceduresCall.insert_Person_field(dtPerson);

            if (dtsbrr != null && dtPerson != null)
                StoredProceduresCall.insert_update_SendBookingRequirementRequests_field(dtsbrr, dtPerson);

            //if (dttr != null)
            //    StoredProceduresCall.insert_update_SendBookingRequirementRequests_field_test(dttr);


        }// public void SendBookingRequirementResponses_field(WebServiceConsumerRequest webServiceConsumerRequest)

        public RequirementResponse[] SendBookingRequirementResponses(WebServiceConsumerRequest webServiceConsumerRequest, Int32 ServiceID)
        {

            RequirementResponse[] rrf = null;
            List<RequirementResponse> list = new List<RequirementResponse>();
            SendBookingRequirementRequest[] SendBookingRequirementRequests = webServiceConsumerRequest.SendBookingRequirementRequests;


            if (SendBookingRequirementRequests == null)
                return null;
                     
            foreach (SendBookingRequirementRequest sbrr in SendBookingRequirementRequests)
            {
                //StoredProceduresCall.insert_update_SendBookingRequirementRequests(sbrr, ServiceID);
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
            SendBookingRequirementResponses_field(webServiceConsumerRequest, ServiceID);
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

                    StoredProceduresCall.insert_BookingResponse(br, ServiceID, Convert.ToInt32(BookingResponseStatus.Accept));
                }
                list.Add(br);
            }

            brfield= list.Cast<BookingResponse>().ToArray();
            return brfield;
        }//public BookingResponse[] AcceptBookingResponses(WebServiceConsumerRequest webServiceConsumerRequest, Int32 ServiceID) {

        public BookingResponse[] CancelBookingResponses(WebServiceConsumerRequest webServiceConsumerRequest, Int32 ServiceID)
        {
            BookingResponse[] brfield = null;
            List<BookingResponse> list = new List<BookingResponse>();
            CancelBookingRequest[] cbrfield = webServiceConsumerRequest.CancelBookingRequests;
            if (cbrfield == null)
                return null;

            foreach (CancelBookingRequest cbr in cbrfield) {

                StoredProceduresCall.insert_CancelBookingRequest(cbr, ServiceID);
                BookingResponse br = new BookingResponse();
                long BookingID = cbr.BookingId;
                if (BookingID != 0) {

                    br.BookingId = BookingID;
                    br.Comment = "";
                    br.IsReceived = true;

                    StoredProceduresCall.insert_BookingResponse(br, ServiceID, Convert.ToInt32(BookingResponseStatus.Cancel));

                }

                list.Add(br);
            }

            brfield = list.Cast<BookingResponse>().ToArray();
            return brfield;
        }//public BookingResponse[] CancelBookingResponses(WebServiceConsumerRequest webServiceConsumerRequest, Int32 ServiceID)

        public BookingResponse[] RequireTicketsResponses(WebServiceConsumerRequest webServiceConsumerRequest, Int32 ServiceID)
        {
            BookingResponse[] brfield = null;
            List<BookingResponse> list = new List<BookingResponse>();
            RequireTicketsRequest[] rtrfield = webServiceConsumerRequest.RequireTicketsRequests;

            if (rtrfield == null)
                return null;

            foreach (RequireTicketsRequest rtr in rtrfield) {
                StoredProceduresCall.insert_RequireTicketsRequest(rtr, ServiceID);
                BookingResponse br = new BookingResponse();
                long BookingID = rtr.BookingId;

                if (BookingID != 0) {

                    br.BookingId = BookingID;
                    br.Comment = "";
                    br.IsReceived = true;

                    StoredProceduresCall.insert_BookingResponse(br, ServiceID, Convert.ToInt32(BookingResponseStatus.RequireTickets));
                }

                list.Add(br);
            }

            brfield = list.Cast<BookingResponse>().ToArray();
            return brfield;
        }//   public BookingResponse[] RequireTicketsResponses(WebServiceConsumerRequest webServiceConsumerRequest, Int32 ServiceID)

    }

}
