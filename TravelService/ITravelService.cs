﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;



namespace TravelService

{

    
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITravelService" in both code and config file together.
    [ServiceContract(Namespace = "http://www.medmar.eu/TravelService_.svc")]
    public interface ITravelService
    {
        /// <summary>
        /// Synchronize is the start point that TA webserivce should expose
        /// This the the main function for the communication between TA and HC system
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json)]
        WebServiceProviderResponse synchronize(WebServiceConsumerRequest webServiceConsumerRequest);

        [OperationContract]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json)]
        String test(Int32 nmb);
    }

   


    /// <summary>
    /// HC generated webservice request
    /// </summary>
    [DataContract]
    public class WebServiceConsumerRequest
    {
        /// <summary>
        /// The credential 
        /// which HC uses to talk with TA webserivce
        /// </summary>
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Password { get; set; }


        /// <summary>
        /// The unique id of current talking 
        /// This Id is generated by HC
        /// </summary>
        [DataMember]
        public long SessionId { get; set; }


        /// <summary>
        /// HC operator require to get some new bookings
        /// So that TA operator could begin to search new bookings by the requirement
        /// </summary>
        [DataMember]
        public SendBookingRequirementRequest[] SendBookingRequirementRequests { get; set; }// old doc called this RequestBookingRequests

        /// <summary>
        /// HC operator require to stop some old requirements
        /// So that TA operator could stop searching bookings by the stoped requirement 
        /// </summary>
        [DataMember]
        public CancelBookingRequirementRequest[] CancelBookingRequirementRequests { get; set; }// old doc called this CancelRequestRequests


        /// <summary>
        /// HC operator require to accepts bookings. 
        /// (Accept means HC operator wants to keep the bookings alive. some of those bookings might be issued in future)
        /// So that TA operator will try to keep the booking alive, meanwhile set booking status to CONFIRMED
        /// TA system will use ConfirmBookingRequests, to confirm this acception. 
        /// </summary>
        [DataMember]
        public AcceptBookingRequest[] AcceptBookingRequests { get; set; }
        /// <summary>
        /// HC operator require to cancels bookings. 
        /// (Cancel means HC operator doesn't want the booking anymore.)
        /// So that TA operator will cancel the booking, meanwhile set booking status to CANCELLED
        /// TA system will use CancelBookingRequests, to confirm this cancelation. 
        /// </summary>
        [DataMember]
        public CancelBookingRequest[] CancelBookingRequests { get; set; }
        /// <summary>
        /// HC operator require to issue bookings. 
        /// (Issue means HC operator want to get ticket for this booking.)
        /// So that TA operator will ticket the booking, meanwhile set booking status to TICKETED
        /// TA system will use SendEticketInfoRequests, to send Eticket info. 
        /// </summary>
        [DataMember]
        public RequireTicketsRequest[] RequireTicketsRequests { get; set; }//old doc called this RequestTicketsRequests

        /// <summary>
        /// Here are Response of TA's requests
        /// (Responses are used to inform TA that the corrosponding TA request was received successfully)
        /// Inside Response:
        ///  IsReceived: corrosponding request is received and saved
        ///  Comment: when IsReceived is false, here will contain the reason why the corrosponding request is not acceptable
        /// </summary>
        [DataMember]
        public BookingResponse[] SendAvailableBookingResponses { get; set; }//old doc called this SendBookingResponses
        [DataMember]
        public BookingResponse[] CancelBookingResponses { get; set; }
        [DataMember]
        public BookingResponse[] ConfirmBookingResponses { get; set; }
        [DataMember]
        public BookingResponse[] UpdateFlightDataResponses { get; set; }
        [DataMember]
        public BookingResponse[] SendEticketInfoResponses { get; set; } //old doc called this SendTicketsResponses
        [DataMember]
        public BookingResponse[] RequestIssuingDecisionResponses { get; set; } //old doc called this RequestIssuingDecisionResponses
    }


    /// <summary>
    /// TA generated webservice response
    /// </summary>
    [DataContract]
    public class WebServiceProviderResponse
    {
        /// <summary>
        /// The unique id of current talking 
        /// This Id is generated by HC
        /// </summary>
        [DataMember]
        public int SessionId { get; set; }

        /// <summary>
        /// TA operator seached and sent new available bookings(WAITLISTED/BOOKED booking) 
        /// according to the HC's SendBookingRequirementRequests
        /// So that HC operator can get those booking options and choose the one they like
        /// </summary>
        [DataMember]
        public SendAvailableBookingRequest[] SendAvailableBookingRequests { get; set; }//old doc called this SendBookingRequests
        /// <summary>
        /// TA operator canceled the bookings 
        /// The booking is canceled is because:
        /// 1. Processed HC's CancelBookingRequests
        /// Or 2. TA operator canceled the booking without HC's CancelBookingRequests (Airline canceled booking, double booking...)
        /// </summary>
        [DataMember]
        public CancelBookingRequest[] CancelBookingRequests { get; set; }
        /// <summary>
        /// TA operator confirmed bookings 
        /// The booking is confirmed is because:
        /// Processed HC's AcceptBookingRequests
        /// </summary>
        [DataMember]
        public ConfirmBookingRequest[] ConfirmBookingRequests { get; set; }
        /// <summary>
        /// TA operator ticketed the booking and send the Eticket
        /// The ticketed is because:
        /// 1.Processed HC's RequireTicketsRequests
        /// Or 2.TA operator have to ticket this booking for keeping the booking alive. 
        /// e.g unticketd but confirmed booking might be expired(not alive) so.
        /// </summary>
        [DataMember]
        public SendEticketInfoRequest[] SendEticketInfoRequests { get; set; }//old doc called this SendTicketsRequests
        /// <summary>
        /// TA operator updated the flight detail of bookings 
        /// The updating is because:
        /// Airline updated the flight detail. e.g. departure time 
        /// </summary>
        [DataMember]
        public UpdateFlightDataRequest[] UpdateFlightDataRequests { get; set; }
        /// <summary>
        /// TA operator require HC operator to make a decision.
        /// The RequireDecision is because:
        /// 1. New booking will be expired (out of time limit) 
        /// but HC operator didn't require to accept/cancel/ticket the booking
        /// 2. The confirmed booking will be expired (out of time limit) 
        /// but HC operator didn't require to cancel/ticket the booking
        /// </summary>
        [DataMember]
        public RequireDecisionRequest[] RequireDecisionRequests { get; set; }//old doc called this RequestIssuingDecisionRequests

        /// <summary>
        /// Here are Response of HC's requests
        /// (Responses are used to inform HC that the corrosponding HC request was received successfully)
        /// Inside Response:
        ///  IsReceived: corrosponding request is received and saved
        ///  Comment: when IsReceived is false, here will contain the reason why the corrosponding request is not acceptable
        /// </summary>
        [DataMember]
        public RequirementResponse[] SendBookingRequirementResponses { get; set; }//old doc called this RequestBookingResponses
        [DataMember]
        public RequirementResponse[] CancelBookingRequirementResponses { get; set; }//old doc called this CancelRequestResponses
        [DataMember]
        public BookingResponse[] AcceptBookingResponses { get; set; }
        [DataMember]
        public BookingResponse[] CancelBookingResponses { get; set; }
        [DataMember]
        public BookingResponse[] RequireTicketsResponses { get; set; }//old doc called this RequestTicketsResponses
    }
    [DataContract]
    public class SendBookingRequirementRequest
    {
        /// <summary>
        /// The unique BookingRequirement Id generate by HC
        /// </summary>
        [DataMember]
        public long BookingRequirementId { get; set; }
        [DataMember]
        public Person Person { get; set; }
        [DataMember]
        public string FromAirport { get; set; }
        [DataMember]
        public string ToAirport { get; set; }
        [DataMember]
        public DateTime? DepartureDate { get; set; }
        [DataMember]
        public DateTime? ArrivalDate { get; set; }

        /// <summary>
        /// Comment used to hold extra info
        /// </summary>
        [DataMember]
        public string RequestBookingComment { get; set; }
    }
    [DataContract]
    public class Person
    {
        [DataMember]
        public long PersonId { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string PassportNumber { get; set; }
        [DataMember]
        public DateTime PassportExpiryDate { get; set; }
        [DataMember]
        public string PassportIssuingCountry { get; set; }
        [DataMember]
        public DateTime Birthday { get; set; }
        [DataMember]
        public string PlaceOfBirth { get; set; }
        [DataMember]
        public string Nationality { get; set; }
        [DataMember]
        public string SeamansBookNumber { get; set; }
        [DataMember]
        public DateTime SeamansBookExpiryDate { get; set; }
        [DataMember]
        public string SeamansBookIssuingCountry { get; set; }
        [DataMember]
        public string USVisaNumber { get; set; }
        [DataMember]
        public DateTime USVisaExpiryDate { get; set; }
        [DataMember]
        public string PersonComment { get; set; }
    }
    [DataContract]
    public class CancelBookingRequirementRequest
    {
        [DataMember]
        public long BookingRequirementId { get; set; }
        [DataMember]
        public string Reason { get; set; }
        [DataMember]
        public string Comment { get; set; }
    }
    [DataContract]
    public class AcceptBookingRequest
    {
        /// <summary>
        /// The unique Booking Id generate by TA
        /// </summary>
        [DataMember]
        public long BookingId { get; set; }
        [DataMember]
        public string Comment { get; set; }
    }
    [DataContract]
    public class CancelBookingRequest
    {
        [DataMember]
        public long BookingId { get; set; }
        [DataMember]
        public string Reason { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public BookingStatus BookingStatus { get; set; }
    }
    [DataContract]
    public class RequireTicketsRequest
    {
        [DataMember]
        public long BookingId { get; set; }
        [DataMember]
        public string Comment { get; set; }
    }
    [DataContract]
    public class BookingResponse
    {
        [DataMember]
        public long BookingId { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public bool IsReceived { get; set; }
    }
    [DataContract]
    public class RequirementResponse
    {
        [DataMember]
        public long BookingRequirementId { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public bool IsReceived { get; set; }
    }
    [DataContract]
    public class SendAvailableBookingRequest
    {
        [DataMember]
        public long BookingRequirementId { get; set; }
        [DataMember]
        public long BookingId { get; set; }
        [DataMember]
        public string FromAirport { get; set; }
        [DataMember]
        public string ToAirport { get; set; }
        [DataMember]
        public DateTime DepartureDate { get; set; }
        [DataMember]
        public DateTime ArrivalDate { get; set; }
        [DataMember]
        public BookingStatus BookingStatus { get; set; }
        [DataMember]
        public DateTime TimeLimit { get; set; }
        [DataMember]
        public string BookingComment { get; set; }
        [DataMember]
        public int SpendTime { get; set; }
        [DataMember]
        public Flight[] Flights { get; set; }
    }
    [DataContract]
    public class Flight
    {
        [DataMember]
        public string FlightId { get; set; }
        [DataMember]
        public string Airline { get; set; }
        [DataMember]
        public string Price { get; set; }
        [DataMember]
        public string Currency { get; set; }
        [DataMember]
        public string FromAirport { get; set; }
        [DataMember]
        public string ToAirport { get; set; }
        [DataMember]
        public DateTime ArrivalDate { get; set; }
        [DataMember]
        public DateTime DepartureDate { get; set; }
        [DataMember]
        public string FlightCode { get; set; }
        [DataMember]
        public string FlightStatus { get; set; }
        [DataMember]
        public string FlightComment { get; set; }
        [DataMember]
        public string Class { get; set; }
        [DataMember]
        public string TicketLocator { get; set; }
        [DataMember]
        public string ETicketNumber { get; set; }
    }
    [DataContract]
    public class ConfirmBookingRequest
    {
        [DataMember]
        public long BookingId { get; set; }
        [DataMember]
        public string BookingComment { get; set; }
        [DataMember]
        public BookingStatus BookingStatus { get; set; }
        [DataMember]
        public DateTime TimeLimit { get; set; }
    }
    [DataContract]
    public class UpdateFlightDataRequest
    {
        [DataMember]
        public long BookingId { get; set; }
        [DataMember]
        public string BookingComment { get; set; }
        [DataMember]
        public BookingStatus BookingStatus { get; set; }
        [DataMember]
        public DateTime TimeLimit { get; set; }
        [DataMember]
        public int SpendTime { get; set; }
        [DataMember]
        public Flight[] Flights { get; set; }
    }
    [DataContract]
    public class SendEticketInfoRequest
    {
        [DataMember]
        public long BookingId { get; set; }
        [DataMember]
        public string BookingComment { get; set; }
        [DataMember]
        public BookingStatus BookingStatus { get; set; }
        [DataMember]
        public ETicket[] ETickets { get; set; }
    }
    [DataContract]
    public class ETicket
    {
        [DataMember]
        public string FlightId { get; set; }
        [DataMember]
        public string ETicketNumber { get; set; }
    }
    [DataContract]
    public class RequireDecisionRequest
    {
        [DataMember]
        public long BookingId { get; set; }
        [DataMember]
        public string BookingComment { get; set; }
        [DataMember]
        public BookingStatus BookingStatus { get; set; }
        [DataMember]
        public DateTime TimeLimit { get; set; }
    }

    [DataContract]
    public enum BookingStatus
    {

        BOOKED = 1,

        WAITLISTED = 2,

        CANCELLED = 3,

        TICKETED = 4,

        CONFIRMED = 5
    }
}
