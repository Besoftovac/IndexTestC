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
                setCulture();

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

        private void setCulture()  {

            String result = StoredProceduresCall.getServerCulture();

            if (result != null)
            {
                if (result == "hrvatski")
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                else
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-CA");
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-CA");
            }

            DateTime D = new DateTime();
            Globals.DefaultDate= D.ToString();

        }
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

       

        public void SendBookingRequirementResponses_field(WebServiceConsumerRequest webServiceConsumerRequest, Int32 ServiceID)
        {           
            List<PersonTA> listPerson = new List<PersonTA>();
            List<SendBookingRequirementRequestTA> listsbrr = new List<SendBookingRequirementRequestTA>();
            //List<TestRequest> listtr = new List<TestRequest>();

            SendBookingRequirementRequest[] sbrrfield = webServiceConsumerRequest.SendBookingRequirementRequests;

            foreach (SendBookingRequirementRequest sbrr in sbrrfield) {                

                SendBookingRequirementRequestTA sbrrTA = InitializeInstances.Initialize_SBRRequestTA(sbrr, ServiceID);
                PersonTA personTA = InitializeInstances.Initialize_PersonTA(sbrr);              

                listPerson.Add(personTA);
                listsbrr.Add(sbrrTA);
        
            }

            DataTable dtsbrr = ToDataTable<SendBookingRequirementRequestTA>(listsbrr);
            DataTable dtPerson = ToDataTable<PersonTA>(listPerson);
        

            if (dtsbrr != null && dtPerson != null)
                StoredProceduresCall.insert_update_SendBookingRequirementRequests_field(dtsbrr, dtPerson);
           


        }// public void SendBookingRequirementResponses_field(WebServiceConsumerRequest webServiceConsumerRequest)

        /*************************************************POČETAK INICIJALNIH METODA*******************************************************/

        public RequirementResponse[] SendBookingRequirementResponses(WebServiceConsumerRequest webServiceConsumerRequest, Int32 ServiceID)
        {

            RequirementResponse[] rrf = null;
            List<RequirementResponseTA> listRRTA = new List<RequirementResponseTA>();//response za komunikaciju sa bazom prema typu koji šaljemo
            List<RequirementResponse> listRR = new List<RequirementResponse>();//za generiranje odgovora servisu prema "njihovoj" definiraniranoj klasi

            SendBookingRequirementRequest[] SendBookingRequirementRequests = webServiceConsumerRequest.SendBookingRequirementRequests;

            if (SendBookingRequirementRequests == null)
                return null;
                     
            foreach (SendBookingRequirementRequest sbrr in SendBookingRequirementRequests)
            {
                RequirementResponseTA rrTA = InitializeInstances.initialize_RequirementResponseTA(sbrr:sbrr);
                RequirementResponse rr = new RequirementResponse();

                    rr.Comment = rrTA.Comment;
                    rr.IsReceived = rrTA.IsReceived;
                    rr.BookingRequirementId = rrTA.BookingRequirementId;

                    listRRTA.Add(rrTA);
                    listRR.Add(rr);
               


            }
            SendBookingRequirementResponses_field(webServiceConsumerRequest, ServiceID);//POZIV PROCEDURE I UPIS ZAHTJEVA U BAZU
            DataTable dtrr = ToDataTable<RequirementResponseTA>(listRRTA);

            StoredProceduresCall.insert_RequirementResponse_field(dtrr);

            rrf = listRR.Cast<RequirementResponse>().ToArray();
            return rrf;

        }// public RequirementResponse[] SendBookingRequirementResponses(WebServiceConsumerRequest webServiceConsumerRequest, Int32 ServiceID)

        public RequirementResponse[] CancelBookingRequirementResponses(WebServiceConsumerRequest webServiceConsumerRequest) {

            RequirementResponse[] rrf = null;
            List<RequirementResponseTA> listRRTA = new List<RequirementResponseTA>();//response za komunikaciju sa bazom prema typu koji šaljemo
            List<RequirementResponse> listRR = new List<RequirementResponse>();//za generiranje odgovora servisu prema "njihovoj" definiraniranoj klasi
            List<CancelBookingRequirementRequestTA> listCbrrTA = new List<CancelBookingRequirementRequestTA>();

            CancelBookingRequirementRequest[] cbrrField = webServiceConsumerRequest.CancelBookingRequirementRequests;

            if (cbrrField == null)
                return null;

            foreach (CancelBookingRequirementRequest cbrr in cbrrField) {

                    CancelBookingRequirementRequestTA cbrrta = InitializeInstances.initialize_cbrrTA(cbrr);
                    RequirementResponseTA rrTA = InitializeInstances.initialize_RequirementResponseTA(cbrr: cbrr);
                    RequirementResponse rr = new RequirementResponse();

                    rr.Comment = rrTA.Comment;
                    rr.IsReceived = rrTA.IsReceived;
                    rr.BookingRequirementId = rrTA.BookingRequirementId;

                    listRRTA.Add(rrTA);
                    listRR.Add(rr);
                    listCbrrTA.Add(cbrrta);
              
            }
            DataTable dtrr = ToDataTable<RequirementResponseTA>(listRRTA);
            DataTable dtcbrr = ToDataTable<CancelBookingRequirementRequestTA>(listCbrrTA);

            if (dtrr!=null)
                StoredProceduresCall.insert_RequirementResponse_field(dtrr);
            if (dtcbrr != null)
                StoredProceduresCall.insert_CancelBookingRequirementRequest_field(dtcbrr);


            rrf = listRR.Cast<RequirementResponse>().ToArray();
            // wpr.r
            return rrf;
        }//public RequirementResponse[] CancelBookingRequirementResponses(WebServiceConsumerRequest webServiceConsumerRequest) {

        public BookingResponse[] AcceptBookingResponses(WebServiceConsumerRequest webServiceConsumerRequest, Int32 ServiceID) {

            BookingResponse[] brfield = null;
            List<BookingResponse> listBR = new List<BookingResponse>();
            List<BookingResponseTA> listBRTA = new List<BookingResponseTA>();
            List<AcceptBookingRequestTA> listabrTA = new List<AcceptBookingRequestTA>();

            AcceptBookingRequest[] abrField = webServiceConsumerRequest.AcceptBookingRequests;

            if (abrField == null)
                return null;

            foreach (AcceptBookingRequest abr in abrField) {
                AcceptBookingRequestTA abrTA = InitializeInstances.initializeAbrTA(abr:abr, ServiceID:ServiceID);
                BookingResponseTA brTA = InitializeInstances.initialize_BookingRespTA(abr: abr, ServiceID:ServiceID);
                BookingResponse br = new BookingResponse();

                br.Comment = brTA.Comment;
                br.IsReceived = brTA.IsReceived;
                br.BookingId = brTA.BookingId;

                listBRTA.Add(brTA);
                listBR.Add(br);
                listabrTA.Add(abrTA);

            }

            DataTable dtbr = ToDataTable<BookingResponseTA>(listBRTA);
            DataTable dtabr = ToDataTable<AcceptBookingRequestTA>(listabrTA);
        
            if (dtbr != null)
                StoredProceduresCall.insert_BookingResponse_field(dtbr);
            if (dtabr != null)
                StoredProceduresCall.insert_AcceptBookingRequest_field(dtabr);

            brfield = listBR.Cast<BookingResponse>().ToArray();
            return brfield;
        }//public BookingResponse[] AcceptBookingResponses(WebServiceConsumerRequest webServiceConsumerRequest, Int32 ServiceID) {

        public BookingResponse[] CancelBookingResponses(WebServiceConsumerRequest webServiceConsumerRequest, Int32 ServiceID)
        {
            BookingResponse[] brfield = null;
            List<BookingResponse> listBR = new List<BookingResponse>();
            List<BookingResponseTA> listBRTA = new List<BookingResponseTA>();
            List<CancelBookingRequestTA> listcbrTA = new List<CancelBookingRequestTA>();


            CancelBookingRequest[] cbrfield = webServiceConsumerRequest.CancelBookingRequests;
            if (cbrfield == null)
                return null;

            foreach (CancelBookingRequest cbr in cbrfield) {

                CancelBookingRequestTA cbrTA = InitializeInstances.initializeCBRTA(cbr: cbr, ServiceID: ServiceID);
                BookingResponseTA brta = InitializeInstances.initialize_BookingRespTA(cbr: cbr, ServiceID: ServiceID);

                BookingResponse br = new BookingResponse();

                br.Comment = brta.Comment;
                br.IsReceived = brta.IsReceived;
                br.BookingId = brta.BookingId;

                listBRTA.Add(brta);
                listBR.Add(br);
                listcbrTA.Add(cbrTA);


            }

            DataTable dtbr = ToDataTable<BookingResponseTA>(listBRTA);
            DataTable dtcbr = ToDataTable<CancelBookingRequestTA>(listcbrTA);

            if (dtbr != null)
                StoredProceduresCall.insert_BookingResponse_field(dtbr);

            if (dtcbr != null)
                StoredProceduresCall.insert_CancelBookingRequest_field(dtcbr);

            brfield = listBR.Cast<BookingResponse>().ToArray();
            return brfield;
        }//public BookingResponse[] CancelBookingResponses(WebServiceConsumerRequest webServiceConsumerRequest, Int32 ServiceID)

        public BookingResponse[] RequireTicketsResponses(WebServiceConsumerRequest webServiceConsumerRequest, Int32 ServiceID)
        {
            BookingResponse[] brfield = null;
            List<BookingResponse> listbr = new List<BookingResponse>();
            List<BookingResponseTA> listbrTA = new List<BookingResponseTA>();
            List<RequireTicketsRequestTA> listrtrTA = new List<RequireTicketsRequestTA>();
            RequireTicketsRequest[] rtrfield = webServiceConsumerRequest.RequireTicketsRequests;

            if (rtrfield == null)
                return null;

            foreach (RequireTicketsRequest rtr in rtrfield) {
                RequireTicketsRequestTA rtrTA = InitializeInstances.intializeRTRTA(rtr: rtr, ServiceID: ServiceID);
                BookingResponseTA brta = InitializeInstances.initialize_BookingRespTA(rtr: rtr, ServiceID: ServiceID);
                BookingResponse br = new BookingResponse();

                br.Comment = brta.Comment;
                br.IsReceived = brta.IsReceived;
                br.BookingId = brta.BookingId;

                listbrTA.Add(brta);
                listbr.Add(br);
                listrtrTA.Add(rtrTA);


            }

            DataTable dtbr = ToDataTable<BookingResponseTA>(listbrTA);
            DataTable dtrtr = ToDataTable<RequireTicketsRequestTA>(listrtrTA);

            if (dtbr != null)
                StoredProceduresCall.insert_BookingResponse_field(dtbr);
            if (dtrtr != null)
                StoredProceduresCall.insert_RequireTicketsRequest_field(dtrtr);

            brfield = listbr.Cast<BookingResponse>().ToArray();
            return brfield;
        }//   public BookingResponse[] RequireTicketsResponses(WebServiceConsumerRequest webServiceConsumerRequest, Int32 ServiceID)

    }

}
