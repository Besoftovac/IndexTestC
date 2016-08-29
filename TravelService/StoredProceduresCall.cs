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
        }

        private static void Execute(SqlCommand cmd, bool bCloseConn = true)
        {
            if (conn != null && conn.State == ConnectionState.Closed)
                conn.Open();
            cmd.ExecuteNonQuery();
            if (bCloseConn) conn.Close();
        }

        public static Int32 InsertService(long SessionID=-1, Int32 Consumer_Id=-1, Int32 Status=-1) {

            Int32 ServiceID = -1;
            try
            {
                SqlCommand cmd = InitSqlCommand("sp_insertIntoService");

                cmd.Parameters.Add("@SessionID", SqlDbType.Int).Value = SessionID;
                cmd.Parameters.Add("@Consumer_Id", SqlDbType.Int).Value = Consumer_Id;
                cmd.Parameters.Add("@ServiceStatus", SqlDbType.Int).Value = Status;


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
                if (sbr.DepartureDate != null && sbr.DepartureDate.ToString() != "1.1.0001. 0:00:00" )                                  
                    cmd.Parameters.Add("@DepartureDate", SqlDbType.DateTime).Value = sbr.DepartureDate;                
                if (sbr.ArrivalDate != null && sbr.ArrivalDate.ToString() != "1.1.0001. 0:00:00")
                    cmd.Parameters.Add("@ArrivalDate", SqlDbType.DateTime).Value = sbr.ArrivalDate;
                if (sbr.BookingRequirementId != 0)
                    cmd.Parameters.Add("@BookingRequirementIdHC", SqlDbType.Int).Value = sbr.BookingRequirementId;
                if (sbr.RequestBookingComment != null)
                    cmd.Parameters.Add("@RequestBookingComment", SqlDbType.NVarChar).Value = sbr.RequestBookingComment;
                
                    cmd.Parameters.Add("@Service_Id", SqlDbType.Int).Value = ServiceID;
                    cmd.Parameters.Add("@Person_Id", SqlDbType.Int).Value = PersonID;

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
                if (person.PassportExpiryDate != null && person.PassportExpiryDate.ToString()!= "1.1.0001. 0:00:00")               
                    cmd.Parameters.Add("@PassportExpiryDate", SqlDbType.DateTime).Value = person.PassportExpiryDate;               
                if (person.PassportIssuingCountry != null)
                    cmd.Parameters.Add("@PassportIssuingCountry", SqlDbType.NVarChar).Value = person.PassportIssuingCountry;
                if (person.Birthday != null && person.Birthday.ToString() != "1.1.0001. 0:00:00")
                    cmd.Parameters.Add("@Birthday", SqlDbType.DateTime).Value = person.Birthday;
                if (person.PlaceOfBirth != null)
                    cmd.Parameters.Add("@PlaceOfBirth", SqlDbType.NVarChar).Value = person.PlaceOfBirth;
                if (person.Nationality != null)
                    cmd.Parameters.Add("@Nationality", SqlDbType.NVarChar).Value = person.Nationality;
                if (person.SeamansBookNumber != null)
                    cmd.Parameters.Add("@SeamansBookNumber", SqlDbType.NVarChar).Value = person.SeamansBookNumber;
                if (person.SeamansBookExpiryDate != null && person.SeamansBookExpiryDate.ToString() != "1.1.0001. 0:00:00")
                    cmd.Parameters.Add("@SeamansBookExpiryDate", SqlDbType.DateTime).Value = person.SeamansBookExpiryDate;
                if (person.SeamansBookIssuingCountry != null)
                    cmd.Parameters.Add("@SeamansBookIssuingCountry", SqlDbType.NVarChar).Value = person.SeamansBookIssuingCountry;
                if (person.USVisaNumber != null)
                    cmd.Parameters.Add("@USVisaNumber", SqlDbType.NVarChar).Value = person.USVisaNumber;
                if (person.USVisaExpiryDate != null && person.USVisaExpiryDate.ToString() != "1.1.0001. 0:00:00")
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

        public static void insert_update_AcceptBookingRequest(AcceptBookingRequest abr, Int32 ServiceID, bool requ=true) {

            try {
                SqlCommand cmd = InitSqlCommand("insert_AcceptBookingRequest");

                //napravi provjere Slavice
                cmd.Parameters.Add("@BookingID", SqlDbType.Int).Value = abr.BookingId;
                cmd.Parameters.Add("@Comment", SqlDbType.NVarChar).Value = abr.Comment;
                cmd.Parameters.Add("@Requ", SqlDbType.Bit).Value = requ;


                Execute(cmd);


            }
            catch (Exception k) {

                throw k;
            }

        }//public static void insert_update_AcceptBookingRequest(AcceptBookingRequest abr) 

        public static void insert_BookingResponse(BookingResponse br, Int32 ServiceID) {

            try {
                SqlCommand cmd = InitSqlCommand("insert_BookingResponse");

                //napravi provjere Slavice
                cmd.Parameters.Add("@BookingID", SqlDbType.Int).Value = br.BookingId;
                cmd.Parameters.Add("@Comment", SqlDbType.NVarChar).Value = br.Comment;
                cmd.Parameters.Add("@IsReceived", SqlDbType.Bit).Value = br.IsReceived;
                cmd.Parameters.Add("@ServiceID", SqlDbType.Int).Value = ServiceID;


                Execute(cmd);


            }
            catch (Exception m)
            {
                throw m;
            }

        }
    }
}
