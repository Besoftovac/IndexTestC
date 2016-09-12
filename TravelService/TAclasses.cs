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

    }
    public class SendBookingRequirementRequestTA
    {
        /// <summary>
        /// The unique BookingRequirement Id generate by HC
        /// </summary>
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
        /*Comment] [varchar](max) NULL,
        [Requ] [bit] NULL,
        [ServiceID] [int] NULL,
        [BookingID] [int] NULL*/
        public string Comment { get; set; }
        public bool Requ { get; set; }
        public Int32 ServiceID { get; set; }
        public long BookingID { get; set; }

    }
}
