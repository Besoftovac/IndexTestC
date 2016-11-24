using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelService
{

    public class PersonTA
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PassportNumber { get; set; }

        public DateTime PassportExpiryDate { get; set; }

        public string PassportIssuingCountry { get; set; }

        public DateTime Birthday { get; set; }

        public string PlaceOfBirth { get; set; }

        public string Nationality { get; set; }

        public string SeamansBookNumber { get; set; }

        public DateTime SeamansBookExpiryDate { get; set; }

        public string SeamansBookIssuingCountry { get; set; }

        public string USVisaNumber { get; set; }

        public DateTime USVisaExpiryDate { get; set; }

        public string PersonComment { get; set; }

        public DateTime Date_ { get; set; }

        public Int32 BookingRequirementIdHC { get; set; }

        public Int32 PersonIDFrontDesk { get; set; }

    }
    public class SendBookingRequirementRequestTA
    {

        public string FromAirport { get; set; }

        public string ToAirport { get; set; }

        public DateTime? DepartureDate { get; set; }

        public DateTime? ArrivalDate { get; set; }

        public Int32 BookingRequirementIdHC { get; set; }


        public string RequestBookingComment { get; set; }

        public Int32 Service_Id { get; set; }

        public Int32 Person_Id { get; set; }

        public DateTime Date_ { get; set; }

        public Int32 Status { get; set; }
    }

    public class TestRequest {

        public string Comment { get; set; }

        public bool Requ { get; set; }

        public Int32 ServiceID { get; set; }

        public long BookingID { get; set; }

    }

    public class RequirementResponseTA {

        public string Comment { get; set; }

        public bool IsReceived { get; set; }

        public Int32 BookingRequirementId { get; set; }

        public Int32 Status { get; set; }

        public DateTime Date_ { get; set; }


    }

    public class CancelBookingRequirementRequestTA
    {     
      
        public string Reason { get; set; }
  
        public string Comment { get; set; }

        public Int32 BookingRequirementId { get; set; }

        public DateTime Date_ { get; set; }

    }

    public class BookingResponseTA {
        
        public string Comment { get; set; }
        
        public bool IsReceived { get; set; }

        public Int32 BookingId { get; set; }

        public Int32 ServiceID { get; set; }

        public DateTime Date_ { get; set; }

        public Int32 BRType { get; set; }

        public bool Requ { get; set; }

    }

    public class AcceptBookingRequestTA {

        public string Comment { get; set; }

        public bool Requ { get; set; }

        public Int32 BookingId { get; set; }

        public Int32 ServiceID { get; set; }

        public DateTime Date_ { get; set; }       

        public bool Seen { get; set; } 

    }

    public class CancelBookingRequestTA
    {  
        public string Reason { get; set; }

        public string Comment { get; set; }

        public Int32 BookingStatusID { get; set; }

        public bool Requ { get; set; }

        public Int32 BookingID { get; set; }

        public Int32 ServiceID { get; set; }

        public DateTime Date_ { get; set; }

        public Int32 Status { get; set; }

    }

    public class RequireTicketsRequestTA {

        public string Comment { get; set; }

        public bool Requ { get; set; }

        public Int32 BookingID { get; set; }

        public Int32 ServiceID { get; set; }

        public DateTime Date_ { get; set; }

        Int32 Status { get; set; }

    }

}//namespace
