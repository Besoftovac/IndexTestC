using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TravelService
{
    class InitializeInstances
    {
        

        static DateTime defaultDate = new DateTime(1756, 1, 1);

        public static PersonTA Initialize_PersonTA(SendBookingRequirementRequest sbrr)
        {

            //Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-CA");

            Person person = sbrr.Person;
            PersonTA personTA = new PersonTA();           


            personTA.FirstName = person.FirstName;
            personTA.LastName = person.LastName;
            personTA.PassportNumber = person.PassportNumber;
            personTA.PassportExpiryDate = (person.PassportExpiryDate == null || person.PassportExpiryDate.ToString() == Globals.DefaultDate) ? defaultDate : person.PassportExpiryDate;
            personTA.PassportIssuingCountry = person.PassportIssuingCountry;
            personTA.Birthday = (person.Birthday == null || person.Birthday.ToString() == Globals.DefaultDate) ? defaultDate : person.Birthday;
            personTA.PlaceOfBirth = person.PlaceOfBirth;
            personTA.Nationality = person.Nationality;
            personTA.SeamansBookNumber = person.SeamansBookNumber;
            personTA.SeamansBookExpiryDate = (person.SeamansBookExpiryDate == null || person.SeamansBookExpiryDate.ToString() == Globals.DefaultDate) ? defaultDate : person.SeamansBookExpiryDate;
            personTA.SeamansBookIssuingCountry = person.SeamansBookIssuingCountry;
            personTA.USVisaNumber = person.USVisaNumber;
            personTA.USVisaExpiryDate = (person.USVisaExpiryDate == null || person.USVisaExpiryDate.ToString() == Globals.DefaultDate) ? defaultDate : person.USVisaExpiryDate;
            personTA.PersonComment = person.PersonComment;
            personTA.Date_ = defaultDate;
            personTA.BookingRequirementIdHC = (Int32)sbrr.BookingRequirementId;

            return personTA;
        }//public PersonTA Initialize_PersonTA(SendBookingRequirementRequest sbrr)

        public static SendBookingRequirementRequestTA Initialize_SBRRequestTA(SendBookingRequirementRequest sbrr, Int32 ServiceID)
        {
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-CA");

            SendBookingRequirementRequestTA sbrrTA = new SendBookingRequirementRequestTA();
          

            sbrrTA.FromAirport = sbrr.FromAirport;
            sbrrTA.ToAirport = sbrr.ToAirport;
            sbrrTA.DepartureDate = (sbrr.DepartureDate == null || sbrr.DepartureDate.ToString() == Globals.DefaultDate) ? defaultDate : sbrr.DepartureDate;
            sbrrTA.ArrivalDate = (sbrr.ArrivalDate == null || sbrr.ArrivalDate.ToString() == Globals.DefaultDate) ? defaultDate : sbrr.ArrivalDate;
            sbrrTA.BookingRequirementIdHC = (Int32)sbrr.BookingRequirementId;
            sbrrTA.RequestBookingComment = sbrr.RequestBookingComment;
            sbrrTA.Service_Id = ServiceID;
            sbrrTA.Person_Id = 0;
            sbrrTA.Date_ = defaultDate;
            sbrrTA.Status = Convert.ToInt32(ServiceStatus.Initial);

            return sbrrTA;

        }

        public static TestRequest initialize_testing(SendBookingRequirementRequest sbrr, Int32 ServiceID)
        {
            TestRequest tr = new TestRequest();

            tr.Comment = "Radi!";
            tr.Requ = true;
            tr.ServiceID = ServiceID;
            tr.BookingID = sbrr.BookingRequirementId;

            return tr;
        }

        public static RequirementResponseTA initialize_RequirementResponseTA(SendBookingRequirementRequest sbrr=null, CancelBookingRequirementRequest cbrr = null) {

            if (sbrr == null && cbrr == null)
                return null;
            else
            {
                RequirementResponseTA rrta = new RequirementResponseTA();

                rrta.Comment = "Initial response";
                rrta.IsReceived = true;
                if (sbrr != null)
                {
                    rrta.BookingRequirementId = (Int32)sbrr.BookingRequirementId;
                    rrta.Status = Convert.ToInt32(RequirementResponseStatus.Send);
                }
                if (cbrr != null) {
                    rrta.BookingRequirementId = (Int32)cbrr.BookingRequirementId;
                    rrta.Status = Convert.ToInt32(RequirementResponseStatus.Cancel);

                }
                rrta.Date_ = defaultDate;

                return rrta;
            }


        }

        public static CancelBookingRequirementRequestTA initialize_cbrrTA(CancelBookingRequirementRequest cbrr) {

            CancelBookingRequirementRequestTA cbrrta = new CancelBookingRequirementRequestTA();
            cbrrta.BookingRequirementId = (Int32)cbrr.BookingRequirementId;
            cbrrta.Comment = cbrr.Comment;
            cbrrta.Reason = cbrr.Reason;
            cbrrta.Date_ = defaultDate;

            return cbrrta;

        }

        public static BookingResponseTA initialize_BookingRespTA(AcceptBookingRequest abr=null, CancelBookingRequest cbr=null, 
            RequireTicketsRequest rtr=null, Int32 ServiceID=-1, bool Requ=true)
        {
            BookingResponseTA brta = new BookingResponseTA();

            if (abr != null) {

                brta.BookingId = (Int32)abr.BookingId;
                brta.BRType = (Int32)BookingResponseType.Accept;
                brta.Comment = abr.Comment;
            }

            if (cbr != null)
            {
                brta.BookingId = (Int32)cbr.BookingId;
                brta.BRType = (Int32)BookingResponseType.Cancel;
                brta.Comment = cbr.Comment;

            }

            if (rtr != null)
            {
                brta.BookingId = (Int32)rtr.BookingId;
                brta.BRType = (Int32)BookingResponseType.RequireTickets;
                brta.Comment = rtr.Comment;

            }

            brta.IsReceived = true;
            brta.Requ = Requ;
            brta.Date_ = defaultDate;
            brta.ServiceID = ServiceID;

            return brta;

        }

        public static AcceptBookingRequestTA initializeAbrTA (AcceptBookingRequest abr=null, Int32 ServiceID = -1, bool Requ = true) {

            AcceptBookingRequestTA abrrta = new AcceptBookingRequestTA();
            abrrta.BookingId = (Int32)abr.BookingId;
            abrrta.Comment = abr.Comment;
            abrrta.Requ = Requ;
            abrrta.ServiceID = ServiceID;
            abrrta.Date_ = defaultDate;

            return abrrta;
        }

        public static CancelBookingRequestTA initializeCBRTA(CancelBookingRequest cbr=null, Int32 ServiceID=-1, bool Requ=true)
        {
            CancelBookingRequestTA cbrTA = new CancelBookingRequestTA();

            cbrTA.Comment = cbr.Comment;
            cbrTA.BookingID = (Int32)cbr.BookingId;
            cbrTA.BookingStatusID = (Int32)cbr.BookingStatus;
            cbrTA.Reason = cbr.Reason;
            cbrTA.Requ = Requ;
            cbrTA.Date_ = defaultDate;
            cbrTA.ServiceID = ServiceID;
            //cbrTA.s

            return cbrTA;

        }
        public static RequireTicketsRequestTA intializeRTRTA(RequireTicketsRequest rtr=null, Int32 ServiceID = -1, bool Requ = true) {

            RequireTicketsRequestTA rtrTA = new RequireTicketsRequestTA();
            rtrTA.BookingID = (Int32) rtr.BookingId;
            rtrTA.Comment = rtr.Comment;
            rtrTA.ServiceID = ServiceID;
            rtrTA.Requ = Requ;
            rtrTA.Date_ = defaultDate;

            return rtrTA;

        }
     
    }
}
