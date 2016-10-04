using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading;
using System.Globalization;
using System.Data.Common;

namespace TravelService
{
    class StoredProceduresCall 
    {

       
        static SqlConnection conn = GeneralSql.CatchDatabase();

        private static SqlCommand InitSqlCommand(String strProcedureName /*SqlConnection conn*/)
        {

            SqlCommand cmd = new SqlCommand(strProcedureName, conn);
            cmd.CommandType = CommandType.StoredProcedure;
           // cmd.Parameters.Add("@Verzija", SqlDbType.Int).Value = (int)verzija_programa.Jedan;

            return cmd;
        }//    private static SqlCommand InitSqlCommand(String strProcedureName /*SqlConnection conn*/)


        private static void Execute(SqlCommand cmd, bool bCloseConn = true)
        {
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-CA");

            if (conn != null && conn.State == ConnectionState.Closed)
                conn.Open();
            cmd.ExecuteNonQuery();
            if (bCloseConn) conn.Close();
        }//private static void Execute(SqlCommand cmd, bool bCloseConn = true)

        public static DataSet response_generateResponse() {
            DataSet DS = new DataSet();
            try
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                //DataSet ds = new DataSet();

                SqlCommand cmd = InitSqlCommand("response_generateResponse");
                da.SelectCommand = cmd;

                da.Fill(DS);
            }
            catch (Exception m)
            {
                throw m;
            }
            finally {
                conn.Close();
            }

            return DS;

        }// public static DataSet response_generateResponse() {

        public static Int32 InsertService(long SessionID=-1, Int32 Consumer_Id=-1) {

            Int32 ServiceID = -1;
            try
            {
                SqlCommand cmd = InitSqlCommand("sp_insertIntoService");

                cmd.Parameters.Add("@SessionID", SqlDbType.Int).Value = SessionID;
                cmd.Parameters.Add("@Consumer_Id", SqlDbType.Int).Value = Consumer_Id;
            


                cmd.Parameters.Add("@ServiceID", SqlDbType.Int).Direction = ParameterDirection.Output;

                Execute(cmd, false);

                ServiceID = Convert.ToInt32(cmd.Parameters["@ServiceID"].Value);
            }
            catch (Exception p)
            {
                throw p;
            }

            finally {
                conn.Close();
            }
            return ServiceID;

        }// public static Int32 InsertService(long SessionID=-1, Int32 Consumer_Id=-1, Int32 Status=-1)

        public static String getServerCulture(Int32 type=1) {
            String result = null;
            try {

                SqlCommand cmd = InitSqlCommand("getServerProperty");
                cmd.Parameters.Add("@Type", SqlDbType.Int).Value = type;
                cmd.Parameters.Add("@result", SqlDbType.VarChar, 1000).Direction = ParameterDirection.Output;

                Execute(cmd, false);

                result = cmd.Parameters["@result"].Value.ToString();
            }
            catch (Exception p) {


                throw p;

            }
            finally
            {
                conn.Close();
            }

            return result;

        }//public static String getServerCulture(Int32 type = 1)
       

        public static void insert_Person_field(DataTable dtPerson) {

            try {
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "insert_Person_field";

                SqlParameter dtPersonType = cmd.Parameters.Add("@PersonType", SqlDbType.Structured);
                dtPersonType.Value = dtPerson;
                dtPersonType.TypeName = "PersonType";

                Execute(cmd);

            }
            catch (Exception m) {

                throw m;

            }

        }//public static void insert_Person_field(DataTable dtPerson) {


        public static void insert_update_SendBookingRequirementRequests_field(DataTable dtRequ, DataTable dtPerson) {

            try {
                if (dtPerson == null || dtRequ == null)
                    return;

                if (dtPerson.Rows.Count < 1 || dtRequ.Rows.Count < 1)
                    return;

                SqlCommand cmd = conn.CreateCommand();               
                   
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "insert_requ_SendBookingRequirementRequest_field";

                SqlParameter dtRequType = cmd.Parameters.Add("@requ_SendBookingRequirementRequestType", SqlDbType.Structured);
                dtRequType.Value = dtRequ;
                dtRequType.TypeName = "requ_SendBookingRequirementRequestType";

                SqlParameter dtPersonType = cmd.Parameters.Add("@PersonType", SqlDbType.Structured);
                dtPersonType.Value = dtPerson;
                dtPersonType.TypeName = "PersonType";

                Execute(cmd);

            }
            catch (Exception m) {

                throw m;
            }

        }//  public static void insert_update_SendBookingRequirementRequests_field(DataTable dtRequ, DataTable dtPerson) {

        public static void insert_RequirementResponse_field(DataTable rr) {

            try {

                if (rr == null || rr.Rows.Count < 1)
                    return;

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "insert_RequirementResponse_field";

                SqlParameter dtRequType = cmd.Parameters.Add("@RequirementResponseType", SqlDbType.Structured);
                dtRequType.Value = rr;
                dtRequType.TypeName = "RequirementResponseType";

                Execute(cmd);

            }
            catch (Exception p) {

                throw p;
            }

        }//public static void insert_RequirementResponse_field(DataTable rr) {


        public static void insert_update_SendBookingRequirementRequests_field_test(DataTable dttr)
        {

            try
            {        
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "insert_requ_SendBookingRequirementRequest_field";

                SqlParameter dtRequType = cmd.Parameters.Add("@TestRequestType", SqlDbType.Structured);
                dtRequType.Value = dttr;
                dtRequType.TypeName = "TestRequestType";


                Execute(cmd);

            }
            catch (Exception m)
            {

                throw m;
            }

        }

        public static void insert_update_SendBookingRequirementRequests(SendBookingRequirementRequest sbr=null, Int32 ServiceID=-1)
        {
            if (sbr == null && ServiceID == -1)
                return;

            try {
                  Int32 PersonID = insert_person(sbr.Person);
                    SqlCommand cmd = InitSqlCommand("insert_requ_SendBookingRequirementRequest");

                if (sbr.FromAirport != null)
                    cmd.Parameters.Add("@FromAirport", SqlDbType.NVarChar).Value = sbr.FromAirport;
                if (sbr.ToAirport != null)
                    cmd.Parameters.Add("@ToAirport", SqlDbType.NVarChar).Value = sbr.ToAirport;
                if (sbr.DepartureDate != null && sbr.DepartureDate.ToString() != Globals.DefaultDate) 
                                                     
                    cmd.Parameters.Add("@DepartureDate", SqlDbType.DateTime).Value = sbr.DepartureDate;                
                if (sbr.ArrivalDate != null && sbr.ArrivalDate.ToString() != Globals.DefaultDate)
                    cmd.Parameters.Add("@ArrivalDate", SqlDbType.DateTime).Value = sbr.ArrivalDate;
                if (sbr.BookingRequirementId != 0)
                    cmd.Parameters.Add("@BookingRequirementIdHC", SqlDbType.Int).Value = sbr.BookingRequirementId;
                if (sbr.RequestBookingComment != null)
                    cmd.Parameters.Add("@RequestBookingComment", SqlDbType.NVarChar).Value = sbr.RequestBookingComment;
                
                    cmd.Parameters.Add("@Service_Id", SqlDbType.Int).Value = ServiceID;
                    cmd.Parameters.Add("@Person_Id", SqlDbType.Int).Value = PersonID;
                    cmd.Parameters.Add("@Status", SqlDbType.Int).Value = Convert.ToInt32(ServiceStatus.Initial);

                Execute(cmd);               
            }
            catch (Exception m) {

                throw m;
            }

        }//public static void insert_update_SendBookingRequirementRequests(SendBookingRequirementRequest[] SendBookingRequirementRequests=null, Int32 ServiceID=-1)

        public static Int32 insert_person(Person person) {

            Int32 PersonID = -1;
            try {
                SqlCommand cmd = InitSqlCommand("insert_Person");

                
                if(person.FirstName!=null)
                    cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = person.FirstName;
                if (person.LastName != null)
                    cmd.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = person.LastName;
                if (person.PassportNumber != null)
                    cmd.Parameters.Add("@PassportNumber", SqlDbType.NVarChar).Value = person.PassportNumber;
                if (person.PassportExpiryDate != null && person.PassportExpiryDate.ToString()!= Globals.DefaultDate)               
                    cmd.Parameters.Add("@PassportExpiryDate", SqlDbType.DateTime).Value = person.PassportExpiryDate;               
                if (person.PassportIssuingCountry != null)
                    cmd.Parameters.Add("@PassportIssuingCountry", SqlDbType.NVarChar).Value = person.PassportIssuingCountry;
                if (person.Birthday != null && person.Birthday.ToString() != Globals.DefaultDate)
                    cmd.Parameters.Add("@Birthday", SqlDbType.DateTime).Value = person.Birthday;
                if (person.PlaceOfBirth != null)
                    cmd.Parameters.Add("@PlaceOfBirth", SqlDbType.NVarChar).Value = person.PlaceOfBirth;
                if (person.Nationality != null)
                    cmd.Parameters.Add("@Nationality", SqlDbType.NVarChar).Value = person.Nationality;
                if (person.SeamansBookNumber != null)
                    cmd.Parameters.Add("@SeamansBookNumber", SqlDbType.NVarChar).Value = person.SeamansBookNumber;
                if (person.SeamansBookExpiryDate != null && person.SeamansBookExpiryDate.ToString() != Globals.DefaultDate)
                    cmd.Parameters.Add("@SeamansBookExpiryDate", SqlDbType.DateTime).Value = person.SeamansBookExpiryDate;
                if (person.SeamansBookIssuingCountry != null)
                    cmd.Parameters.Add("@SeamansBookIssuingCountry", SqlDbType.NVarChar).Value = person.SeamansBookIssuingCountry;
                if (person.USVisaNumber != null)
                    cmd.Parameters.Add("@USVisaNumber", SqlDbType.NVarChar).Value = person.USVisaNumber;
                if (person.USVisaExpiryDate != null && person.USVisaExpiryDate.ToString() != Globals.DefaultDate)
                    cmd.Parameters.Add("@USVisaExpiryDate", SqlDbType.DateTime).Value = person.USVisaExpiryDate;
                if (person.PersonComment != null)
                    cmd.Parameters.Add("@PersonComment", SqlDbType.NVarChar).Value = person.PersonComment;


               cmd.Parameters.Add("@PersonID", SqlDbType.Int).Direction = ParameterDirection.Output;

                Execute(cmd, false);

                PersonID = Convert.ToInt32(cmd.Parameters["@PersonID"].Value);
            }

            catch (Exception m) {
                throw m;
            }

            finally
            {
                conn.Close();
            }

            return PersonID;

        }// public static Int32 insert_person(Person person)

        public static void insert_RequirementResponse(RequirementResponse rr, Int32 status) {

            try {

                SqlCommand cmd = InitSqlCommand("insert_RequirementResponse");

              
                    cmd.Parameters.Add("@BookingRequirementId", SqlDbType.Int).Value = rr.BookingRequirementId;               
                    cmd.Parameters.Add("@IsReceived", SqlDbType.Bit).Value = rr.IsReceived;
                    cmd.Parameters.Add("@Comment", SqlDbType.NVarChar).Value = rr.Comment;
                    cmd.Parameters.Add("@Status", SqlDbType.Int).Value = status;

                    Execute(cmd);

            }
            catch (Exception m) {

                throw m;
            }
        }

        public static void insert_update_CancelBookingRequirementRequest(CancelBookingRequirementRequest CBRR) {

            try {

                SqlCommand cmd = InitSqlCommand("insert_CancelBookingRequirementRequest");

                //napravi provjere Slavice
                cmd.Parameters.Add("@BookingRequirementId", SqlDbType.Int).Value = CBRR.BookingRequirementId;
                cmd.Parameters.Add("@Reason", SqlDbType.NVarChar).Value = CBRR.Reason;
                cmd.Parameters.Add("@Comment", SqlDbType.NVarChar).Value = CBRR.Comment;
              

                Execute(cmd);
            }
            catch (Exception m) {

                throw m;
            }

        }//public static void insert_update_CancelBookingRequirementRequest(CancelBookingRequirementRequest CBRR) 

        public static void insert_CancelBookingRequirementRequest_field(DataTable dtcbrr) {

            try {
                if (dtcbrr.Rows.Count < 1 || dtcbrr==null)
                    return;

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "insert_CancelBookingRequirementRequest_field";
                cmd.Parameters.Add("@CommentLogUser", SqlDbType.Int).Value = Convert.ToInt32(CommentLogUsers.HC);

                SqlParameter dtRequType = cmd.Parameters.Add("@CancelBookingRequirementRequestType", SqlDbType.Structured);
                dtRequType.Value = dtcbrr;
                dtRequType.TypeName = "CancelBookingRequirementRequestType";

                Execute(cmd);
            }
            catch (Exception p)
            {
                throw p;
            }
        }

        public static void insert_update_AcceptBookingRequest(AcceptBookingRequest abr, Int32 ServiceID, bool requ=true) {

            try {
                SqlCommand cmd = InitSqlCommand("insert_AcceptBookingRequest");

                if (abr.BookingId!=0)
                    cmd.Parameters.Add("@BookingID", SqlDbType.Int).Value = abr.BookingId;
                if(abr.Comment!=null)
                    cmd.Parameters.Add("@Comment", SqlDbType.NVarChar).Value = abr.Comment;
          
                cmd.Parameters.Add("@Requ", SqlDbType.Bit).Value = requ;
                cmd.Parameters.Add("@ServiceID", SqlDbType.NVarChar).Value = ServiceID;

                //@ServiceID


                Execute(cmd);


            }
            catch (Exception k) {

                throw k;
            }

        }//public static void insert_update_AcceptBookingRequest(AcceptBookingRequest abr) 

        public static void insert_AcceptBookingRequest_field(DataTable dtabr) {

            try {
                if (dtabr.Rows.Count < 1 || dtabr == null)
                    return;

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "insert_AcceptBookingRequest_field";
                cmd.Parameters.Add("@CommentLogUser", SqlDbType.Int).Value = Convert.ToInt32(CommentLogUsers.HC);

                SqlParameter dtRequType = cmd.Parameters.Add("@AcceptBookingRequestType", SqlDbType.Structured);
                dtRequType.Value = dtabr;
                dtRequType.TypeName = "AcceptBookingRequestType";

                Execute(cmd);
            }

            catch (Exception m) {

                throw m;
            }
        }

        public static void insert_BookingResponse(BookingResponse br, Int32 ServiceID, Int32 BRType, Int32 CommentLogUser, bool Requ=true)
        {
            try {
                
                SqlCommand cmd = InitSqlCommand("insert_BookingResponse");

                //napravi provjere Slavice
                cmd.Parameters.Add("@BookingID", SqlDbType.Int).Value = br.BookingId;
                cmd.Parameters.Add("@Comment", SqlDbType.NVarChar).Value = br.Comment;
                cmd.Parameters.Add("@IsReceived", SqlDbType.Bit).Value = br.IsReceived;
                cmd.Parameters.Add("@ServiceID", SqlDbType.Int).Value = ServiceID;
                cmd.Parameters.Add("@BRType", SqlDbType.Int).Value = BRType;
                cmd.Parameters.Add("@CommentLogUser", SqlDbType.Int).Value = CommentLogUser;
                cmd.Parameters.Add("@Requ", SqlDbType.Int).Value = Requ;

                Execute(cmd);
            }
            catch (Exception m)
            {
                throw m;
            }

        }

        public static void insert_BookingResponse_field(DataTable dtbr, Int32 who) {//who - inicijalni odgovor od stran HC-a ili nas 


            try {
                if (dtbr.Rows.Count < 1 || dtbr == null)
                    return;

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "insert_BookingResponse_field";
                cmd.Parameters.Add("@CommentLogUser", SqlDbType.Int).Value = who; //Convert.ToInt32(CommentLogUsers.HC);

                SqlParameter dtRequType = cmd.Parameters.Add("@BookingResponseType", SqlDbType.Structured);
                dtRequType.Value = dtbr;
                dtRequType.TypeName = "BookingResponseType";

                Execute(cmd);


            }
            catch (Exception k) {

                throw k;
            }
        }

        public static void insert_CancelBookingRequest(CancelBookingRequest cbr, Int32 ServiceID,  bool requ = true) {

            try
            {
                SqlCommand cmd = InitSqlCommand("insert_CancelBookingRequest");

                //napravi provjere Slavice
                if(cbr.BookingId!=0)
                    cmd.Parameters.Add("@BookingID", SqlDbType.Int).Value = cbr.BookingId;
                if(cbr.Comment!=null)
                    cmd.Parameters.Add("@Comment", SqlDbType.NVarChar).Value = cbr.Comment;
                if(cbr.Reason!=null)
                    cmd.Parameters.Add("@Reason", SqlDbType.NVarChar).Value = cbr.Reason;
                if (cbr.BookingStatus != 0)
                    cmd.Parameters.Add("@BookingStatusID", SqlDbType.Int).Value = cbr.BookingStatus;


                cmd.Parameters.Add("@ServiceID", SqlDbType.Int).Value = ServiceID;
                cmd.Parameters.Add("@Requ", SqlDbType.Bit).Value = ServiceID;       

                Execute(cmd);


            }
            catch (Exception m)
            {
                throw m;
            }
        }
        public static void insert_CancelBookingRequest_field(DataTable dtcbr) {

            try {
                if (dtcbr.Rows.Count < 1 || dtcbr == null)
                    return;

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "insert_CancelBookingRequest_field";
                cmd.Parameters.Add("@CommentLogUser", SqlDbType.Int).Value = Convert.ToInt32(CommentLogUsers.HC);

                SqlParameter dtRequType = cmd.Parameters.Add("@CancelBookingRequestType", SqlDbType.Structured);
                dtRequType.Value = dtcbr;
                dtRequType.TypeName = "CancelBookingRequestType";

                Execute(cmd);

            }
            catch (Exception m) {
                throw m;
            }
        }
        public static void insert_RequireTicketsRequest(RequireTicketsRequest rtr, Int32 ServiceID, bool requ=true) {
            try
            {
                SqlCommand cmd = InitSqlCommand("insert_RequireTicketsRequest");

                if (rtr.BookingId!=0)
                    cmd.Parameters.Add("@BookingID", SqlDbType.Int).Value = rtr.BookingId;
                if(rtr.Comment!=null)
                    cmd.Parameters.Add("@Comment", SqlDbType.NVarChar).Value = rtr.Comment;

                cmd.Parameters.Add("@Requ", SqlDbType.Bit).Value = requ;
                cmd.Parameters.Add("@ServiceID", SqlDbType.Int).Value = ServiceID;       

                Execute(cmd);
            }
            catch (Exception m)
            {
                throw m;
            }
        }

        public static void insert_RequireTicketsRequest_field(DataTable dtrtr) {
            try
            {
                if (dtrtr.Rows.Count < 1 || dtrtr == null)
                    return;

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "insert_RequireTicketsRequest_field";
                cmd.Parameters.Add("@CommentLogUser", SqlDbType.Int).Value = Convert.ToInt32(CommentLogUsers.HC);

                SqlParameter dtRequType = cmd.Parameters.Add("@RequireTicketsRequestType", SqlDbType.Structured);
                dtRequType.Value = dtrtr;
                dtRequType.TypeName = "RequireTicketsRequestType";

                Execute(cmd);

            }
            catch (Exception m)
            {
                throw m;
            }

        }
    }
}
