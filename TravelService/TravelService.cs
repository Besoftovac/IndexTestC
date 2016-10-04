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
                Globals.ResponseTables = StoredProceduresCall.response_generateResponse();
                long SessionID = webServiceConsumerRequest.SessionId;

                Int32 ServiceID = StoredProceduresCall.InsertService(SessionID: SessionID);
                setCulture();

                WebServiceProviderResponse WPR = new WebServiceProviderResponse();

                WPR.SessionId = (Int32)SessionID;

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

                if (Globals.ResponseTables != null || Globals.ResponseTables.Tables.Count > 0){

                    WPR.SendAvailableBookingRequests = AvailableBookings(Globals.ResponseTables);
                    WPR.RequireDecisionRequests = RequireDecision(Globals.ResponseTables);
                    WPR.CancelBookingRequests = CancelBooking(Globals.ResponseTables);
                    WPR.ConfirmBookingRequests = ConfirmBooking(Globals.ResponseTables);
                    WPR.SendEticketInfoRequests = EticketInfo(Globals.ResponseTables);
                    WPR.UpdateFlightDataRequests = UpdateFlightData(Globals.ResponseTables);
                }

                storeInitial_requResponse(webServiceConsumerRequest, ServiceID);

                return WPR;
            }// if (webServiceConsumerRequest != null)
            else
                return null;

        }// public WebServiceProviderResponse synchronize(WebServiceConsumerRequest webServiceConsumerRequest)
        #region GLOBAL SERVICE
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

        }// private void setCulture()

        public static DateTime date(DateTime date) {

            DateTime dt;
            bool success = DateTime.TryParseExact(date.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt);
            if (success)
            {
                var result = dt.ToString("yyyy-MM-dd");
            }

            return dt;
        }// public static DateTime date(DateTime date) {

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
        }// public static DataTable ToDataTable<T>(List<T> items)
        #endregion


        #region RESPONSE METHODS - TA SERVICE DATA
        public UpdateFlightDataRequest[] UpdateFlightData(DataSet ds) {
            if ((ds == null) || (ds.Tables.Count == 0))
                return null;

            UpdateFlightDataRequest[] UpFDR = null;
            List<Flight> listFlights = new List<Flight>();
            List<UpdateFlightDataRequest> listUpFDR = new List<UpdateFlightDataRequest>();
            DataTable dtUpFDR = new DataTable();
            dtUpFDR = ds.Tables[8];

            if (dtUpFDR == null || dtUpFDR.Rows.Count < 1)
                return null;
            /********************************treba preraditi taj dio u proceduri******************************************/

            ds.Relations.Add("UpdateFlight", ds.Tables[8].Columns["BookingID"], ds.Tables[7].Columns["BookingID"]);

            foreach (DataRow dr in dtUpFDR.Rows)
            {

                UpdateFlightDataRequest ufdr = new UpdateFlightDataRequest();

                ufdr.BookingComment = (dr["BookingComment"] == DBNull.Value) ? ""
                : (dr["BookingComment"]).ToString();
                ufdr.BookingId = (dr["BookingID"] == DBNull.Value) ? -1
                : Convert.ToInt32(dr["BookingID"]);
                ufdr.BookingStatus = (dr["BookingStatusID"] == DBNull.Value) ? BookingStatus.BOOKED
                : (BookingStatus)(dr["BookingStatusID"]);
                ufdr.SpendTime = (dr["SpendTime"] == DBNull.Value) ? -1
                : Convert.ToInt32(dr["SpendTime"]);
                ufdr.TimeLimit = (dr["TimeLimit"] == DBNull.Value) ? Convert.ToDateTime(Globals.DefaultDate)
                : Convert.ToDateTime(dr["TimeLimit"]);

                listFlights.Clear();

                foreach (DataRow drChild in dr.GetChildRows("UpdateFlight"))
                {

                    Flight fl = new Flight();
                    fl.FlightId = (drChild["Id"] == DBNull.Value) ? "-1"
                    : (drChild["Id"]).ToString();
                    fl.FlightStatus = (drChild["FlightStatus"] == DBNull.Value) ? " "
                    : (drChild["FlightStatus"]).ToString();
                    fl.Airline = (drChild["Airline"] == DBNull.Value) ? " "
                    : (drChild["Airline"]).ToString();
                    fl.Price = (drChild["Price"] == DBNull.Value) ? " "
                    : (drChild["Price"]).ToString();
                    fl.Currency = (drChild["Currency"] == DBNull.Value) ? " "
                    : (drChild["Currency"]).ToString();
                    fl.FromAirport = (drChild["FromAirport"] == DBNull.Value) ? " "
                   : (drChild["FromAirport"]).ToString();
                    fl.ToAirport = (drChild["ToAirport"] == DBNull.Value) ? " "
                   : (drChild["ToAirport"]).ToString();
                    fl.ArrivalDate = (drChild["ArrivalDate"] == DBNull.Value) ? Convert.ToDateTime(Globals.DefaultDate)
                    : Convert.ToDateTime(drChild["ArrivalDate"]);
                    fl.DepartureDate = (drChild["DepartureDate"] == DBNull.Value) ? Convert.ToDateTime(Globals.DefaultDate)
                    : Convert.ToDateTime(drChild["DepartureDate"]);
                    fl.FlightCode = (drChild["FlightCode"] == DBNull.Value) ? ""
                   : (drChild["FlightCode"]).ToString();
                    fl.FlightComment = (drChild["FlightComment"] == DBNull.Value) ? ""
                  : (drChild["FlightComment"]).ToString();
                    fl.Class = (drChild["Class"] == DBNull.Value) ? ""
                    : (drChild["Class"]).ToString();
                    fl.TicketLocator = (drChild["TicketLocator"] == DBNull.Value) ? ""
                : (drChild["TicketLocator"]).ToString();
                    fl.ETicketNumber = (drChild["ETicketNumber"] == DBNull.Value) ? ""
               : (drChild["ETicketNumber"]).ToString();

                    listFlights.Add(fl);


                }

                ufdr.Flights = listFlights.Cast<Flight>().ToArray();

                listUpFDR.Add(ufdr);


            }

            UpFDR = listUpFDR.Cast<UpdateFlightDataRequest>().ToArray();
            return UpFDR;

        }//public UpdateFlightDataRequest[] UpdateFlightData(DataSet ds) {

        public SendEticketInfoRequest[] EticketInfo(DataSet ds)
        {
            if ((ds == null) || (ds.Tables.Count == 0))
                return null;

            SendEticketInfoRequest[] setRequest = null;
            List<SendEticketInfoRequest> listSetinfo = new List<SendEticketInfoRequest>();
            List<ETicket> listEticket = new List<ETicket>();

            DataTable setinfo = new DataTable();
            setinfo = ds.Tables[6];

            if (setinfo == null || setinfo.Rows.Count < 1)
                return null;

            ds.Relations.Add("Eticket", ds.Tables[6].Columns["Id"], ds.Tables[5].Columns["SendEticketInfoRequest_Id"]);

            foreach (DataRow dr in setinfo.Rows)
            {
                SendEticketInfoRequest seir = new SendEticketInfoRequest();
                seir.BookingComment = (dr["BookingComment"] == DBNull.Value) ? ""
                : (dr["BookingComment"]).ToString();
                seir.BookingId = (dr["BookingID"] == DBNull.Value) ? -1
                : Convert.ToInt32(dr["BookingID"]);
                seir.BookingStatus = (dr["BookingStatusID"] == DBNull.Value) ? BookingStatus.BOOKED
                : (BookingStatus)(dr["BookingStatusID"]);

                listEticket.Clear();

                foreach (DataRow drChild in dr.GetChildRows("Eticket"))
                {
                    ETicket eticket = new ETicket();

                    eticket.ETicketNumber = (drChild["ETicketNumber"] == DBNull.Value) ? ""
                    : (drChild["ETicketNumber"]).ToString();
                    eticket.FlightId = (drChild["Flight_Id"] == DBNull.Value) ? "-1"
                    : (drChild["Flight_Id"]).ToString();

                    listEticket.Add(eticket);
                }//foreach (DataRow drChild in dr.GetChildRows("Eticket"))

                seir.ETickets = listEticket.Cast<ETicket>().ToArray();
                listSetinfo.Add(seir);
            }// foreach (DataRow dr in setinfo.Rows)

            setRequest = listSetinfo.Cast<SendEticketInfoRequest>().ToArray();
            return setRequest;
        }//public SendEticketInfoRequest[] EticketInfo(DataSet ds)


        public ConfirmBookingRequest[] ConfirmBooking(DataSet ds) {
            if ((ds == null) || (ds.Tables.Count == 0))
                return null;

            ConfirmBookingRequest[] conBRequests = null;
            List<ConfirmBookingRequest> listconbr = new List<ConfirmBookingRequest>();
            DataTable conbRequ = new DataTable();
            conbRequ = ds.Tables[4];

            if (conbRequ == null || conbRequ.Rows.Count < 1)
                return null;
            foreach (DataRow dr in conbRequ.Rows) {

                ConfirmBookingRequest conbr = new ConfirmBookingRequest();

                conbr.BookingComment= (dr["BookingComment"] == DBNull.Value) ? ""
                : (dr["BookingComment"]).ToString();
                conbr.BookingId = (dr["BookingID"] == DBNull.Value) ? -1
                : Convert.ToInt32(dr["BookingID"]);
                conbr.BookingStatus = (dr["BookingStatusID"] == DBNull.Value) ? BookingStatus.BOOKED
                : (BookingStatus)(dr["BookingStatusID"]);
                conbr.TimeLimit = (dr["TimeLimit"] == DBNull.Value) ? Convert.ToDateTime(Globals.DefaultDate)
                : Convert.ToDateTime(dr["TimeLimit"]);

                listconbr.Add(conbr);

            }
            conBRequests = listconbr.Cast<ConfirmBookingRequest>().ToArray();
            return conBRequests;
        }// public ConfirmBookingRequest[] ConfirmBooking(DataSet ds) {


        public CancelBookingRequest[] CancelBooking(DataSet ds) {
            if ((ds == null) || (ds.Tables.Count == 0))
                return null;

            CancelBookingRequest[] canBRequests = null;
            List<CancelBookingRequest> listcanbr = new List<CancelBookingRequest>();
            DataTable canBRequ = new DataTable();
            canBRequ = ds.Tables[3];

            if (canBRequ == null || canBRequ.Rows.Count < 1)
                return null;

            foreach (DataRow dr in canBRequ.Rows) {

                CancelBookingRequest canbr = new CancelBookingRequest();
                 canbr.BookingId = (dr["BookingID"] == DBNull.Value) ? -1
                : Convert.ToInt32(dr["BookingID"]);
                canbr.BookingStatus = (dr["BookingStatusID"] == DBNull.Value) ? BookingStatus.BOOKED
                : (BookingStatus)(dr["BookingStatusID"]);
                canbr.Comment = (dr["Comment"] == DBNull.Value) ? ""
                : (dr["Comment"]).ToString();
                canbr.Reason = (dr["Reason"] == DBNull.Value) ? ""
                : (dr["Reason"]).ToString();

                listcanbr.Add(canbr);
            }

            canBRequests = listcanbr.Cast<CancelBookingRequest>().ToArray();
            return canBRequests;
        }// public CancelBookingRequest[] CancelBooking(DataSet ds) {

        public RequireDecisionRequest[] RequireDecision(DataSet ds) { 
            if ((ds == null) || (ds.Tables.Count == 0))
                return null;

            RequireDecisionRequest[] rdrequests = null;
            List<RequireDecisionRequest> listrdr = new List<RequireDecisionRequest>();
            DataTable rdrequ = new DataTable();
            rdrequ = ds.Tables[2];

            if (rdrequ == null || rdrequ.Rows.Count < 1)
                return null;

            foreach (DataRow dr in rdrequ.Rows) {

                RequireDecisionRequest rdr = new RequireDecisionRequest();
                rdr.BookingId = (dr["BookingID"] == DBNull.Value) ? -1
                : Convert.ToInt32(dr["BookingID"]);
                rdr.BookingComment = (dr["BookingComment"] == DBNull.Value) ? ""
                : (dr["BookingComment"]).ToString();
                rdr.BookingStatus = (dr["BookingStatusID"] == DBNull.Value) ? BookingStatus.BOOKED
                : (BookingStatus)(dr["BookingStatusID"]);
                rdr.TimeLimit = (dr["TimeLimit"] == DBNull.Value) ? Convert.ToDateTime(Globals.DefaultDate)
                : Convert.ToDateTime(dr["TimeLimit"]);

                listrdr.Add(rdr);

            }
            rdrequests=listrdr.Cast<RequireDecisionRequest>().ToArray();
            return rdrequests;
        }//  public RequireDecisionRequest[] RequireDecision(DataSet ds) {


        public SendAvailableBookingRequest[] AvailableBookings(DataSet ds) {

            if ((ds == null) || (ds.Tables.Count == 0))
                return null;


            SendAvailableBookingRequest[] bookings = null;
            
            List<Flight> listFlight = new List<Flight>();
            List<SendAvailableBookingRequest> listBooking = new List<SendAvailableBookingRequest>();


            DataTable responses = new DataTable();
            DataTable flights = new DataTable();

            responses = ds.Tables[1];
            flights = ds.Tables[0];

            if ((responses == null) || (responses.Rows.Count < 1))
                return null;

            if ((flights == null) || (flights.Rows.Count < 1))
                return null;

            ds.Relations.Add("Response_flight", ds.Tables[1].Columns["ID"], ds.Tables[0].Columns["BookingID"]);

            foreach (DataRow dr in responses.Rows) {

                SendAvailableBookingRequest sabr = new SendAvailableBookingRequest();
                sabr.ArrivalDate = (dr["ArrivalDate"] == DBNull.Value) ? Convert.ToDateTime(Globals.DefaultDate)
                    : Convert.ToDateTime(dr["ArrivalDate"]);
                sabr.DepartureDate = (dr["DepartureDate"] == DBNull.Value) ? Convert.ToDateTime(Globals.DefaultDate)
                    : Convert.ToDateTime(dr["DepartureDate"]);
                sabr.BookingId= (dr["Id"] == DBNull.Value) ? -1
                    : Convert.ToInt32(dr["Id"]);
                sabr.BookingRequirementId = (dr["BookingRequirementIdHC"] == DBNull.Value) ? -1
                    : Convert.ToInt32(dr["BookingRequirementIdHC"]);
                sabr.BookingComment = (dr["BookingComment"] == DBNull.Value) ? ""
                    : (dr["BookingComment"]).ToString();
                sabr.FromAirport = (dr["FromAirport"] == DBNull.Value) ? ""
                    : (dr["FromAirport"]).ToString();
                sabr.FromAirport = (dr["ToAirport"] == DBNull.Value) ? ""
                    : (dr["ToAirport"]).ToString();
                sabr.BookingStatus = (dr["BookingStatusID"] == DBNull.Value) ? BookingStatus.BOOKED
                    : (BookingStatus)(dr["BookingStatusID"]);
                sabr.TimeLimit = (dr["TimeLimit"] == DBNull.Value) ? Convert.ToDateTime(Globals.DefaultDate)
                    : Convert.ToDateTime(dr["TimeLimit"]);
                sabr.SpendTime = (dr["SpendTime"] == DBNull.Value) ? -1
                : Convert.ToInt32(dr["SpendTime"]);

                listFlight.Clear();
                foreach (DataRow drChild in dr.GetChildRows("Response_flight")) {

                    Flight flight = new Flight();
                    flight.FlightId = (drChild["Id"] == DBNull.Value) ? "-1"
                    : (drChild["Id"]).ToString();
                    flight.FlightStatus= (drChild["FlightStatus"] == DBNull.Value) ? " "
                    : (drChild["FlightStatus"]).ToString();
                    flight.Airline=(drChild["Airline"] == DBNull.Value) ? " "
                    : (drChild["Airline"]).ToString();
                    flight.Price = (drChild["Price"] == DBNull.Value) ? " "
                    : (drChild["Price"]).ToString();
                    flight.Currency = (drChild["Currency"] == DBNull.Value) ? " "
                    : (drChild["Currency"]).ToString();
                    flight.FromAirport = (drChild["FromAirport"] == DBNull.Value) ? " "
                   : (drChild["FromAirport"]).ToString();
                    flight.ToAirport = (drChild["ToAirport"] == DBNull.Value) ? " "
                   : (drChild["ToAirport"]).ToString();
                    flight.ArrivalDate = (drChild["ArrivalDate"] == DBNull.Value) ? Convert.ToDateTime(Globals.DefaultDate)
                    : Convert.ToDateTime(drChild["ArrivalDate"]);
                    flight.DepartureDate = (drChild["DepartureDate"] == DBNull.Value) ? Convert.ToDateTime(Globals.DefaultDate)
                    : Convert.ToDateTime(drChild["DepartureDate"]);
                    flight.FlightCode = (drChild["FlightCode"] == DBNull.Value) ? ""
                   : (drChild["FlightCode"]).ToString();
                    flight.FlightComment = (drChild["FlightComment"] == DBNull.Value) ? ""
                  : (drChild["FlightComment"]).ToString();
                    flight.Class = (drChild["Class"] == DBNull.Value) ? ""
                 : (drChild["Class"]).ToString();
                    flight.TicketLocator = (drChild["TicketLocator"] == DBNull.Value) ? ""
                : (drChild["TicketLocator"]).ToString();
                    flight.ETicketNumber = (drChild["ETicketNumber"] == DBNull.Value) ? ""
               : (drChild["ETicketNumber"]).ToString();

                    listFlight.Add(flight);

                }//flights
              
                sabr.Flights = listFlight.Cast<Flight>().ToArray();               
                listBooking.Add(sabr);
            }//SendAvailableBookingRequest

            bookings = listBooking.Cast<SendAvailableBookingRequest>().ToArray();
            return bookings;
        }//  public SendAvailableBookingRequest[] AvailableBookings(DataSet ds) {
        #endregion

        #region INIIAL RESPONSE - STORE REQUEST TO DATABASE

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
                StoredProceduresCall.insert_BookingResponse_field(dtbr, (Int32)CommentLogUsers.TA);
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
                StoredProceduresCall.insert_BookingResponse_field(dtbr, (Int32)CommentLogUsers.TA);

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
                StoredProceduresCall.insert_BookingResponse_field(dtbr, (Int32)CommentLogUsers.TA);
            if (dtrtr != null)
                StoredProceduresCall.insert_RequireTicketsRequest_field(dtrtr);

            brfield = listbr.Cast<BookingResponse>().ToArray();
            return brfield;
        }//   public BookingResponse[] RequireTicketsResponses(WebServiceConsumerRequest webServiceConsumerRequest, Int32 ServiceID)
        #endregion

        #region INTITAL REQUEST RESPONSE

        public void storeInitial_requResponse(WebServiceConsumerRequest webSCR, Int32 ServiceID) {

            if (webSCR.SendAvailableBookingResponses != null)
                HC_response(webSCR.SendAvailableBookingResponses, (Int32)BookingResponseType.SendAvailableBooking, ServiceID);

            if (webSCR.CancelBookingResponses != null)
                HC_response(webSCR.CancelBookingResponses, (Int32)BookingResponseType.CancelBooking, ServiceID);

            if (webSCR.ConfirmBookingResponses != null)
                HC_response(webSCR.ConfirmBookingResponses, (Int32)BookingResponseType.ConfirmBooking, ServiceID);

            if (webSCR.UpdateFlightDataResponses != null)
                HC_response(webSCR.UpdateFlightDataResponses, (Int32)BookingResponseType.UpdateFlightData, ServiceID);

            if (webSCR.SendEticketInfoResponses != null)
                HC_response(webSCR.SendEticketInfoResponses, (Int32)BookingResponseType.EticketInfo, ServiceID);

            if (webSCR.RequestIssuingDecisionResponses != null)
                HC_response(webSCR.RequestIssuingDecisionResponses, (Int32)BookingResponseType.DecisionResponses, ServiceID);
        }

        private void HC_response(BookingResponse[] brField, Int32 BrType, Int32 ServiceID, bool requ= true) {

           DateTime defaultDate = new DateTime(1756, 1, 1);

            if (brField == null || BrType == 0)
                return;

            List<BookingResponseTA> listbrTA = new List<BookingResponseTA>();

            foreach (BookingResponse br in brField) {
                BookingResponseTA brta = new BookingResponseTA();

                brta.BookingId = (Int32)br.BookingId;
                brta.BRType = BrType;
                brta.Comment = br.Comment;
                brta.IsReceived = br.IsReceived;
                brta.Requ = requ;
                brta.Date_ = defaultDate; // Convert.ToDateTime(Globals.DefaultDate);
                brta.ServiceID = ServiceID;

                listbrTA.Add(brta);
            }

            DataTable dtbr = ToDataTable<BookingResponseTA>(listbrTA);

            if (dtbr != null)
                StoredProceduresCall.insert_BookingResponse_field(dtbr, (Int32)CommentLogUsers.HC);

        }


        #endregion

    }

}
