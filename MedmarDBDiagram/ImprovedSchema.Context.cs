﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MedmarDBDiagram
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MedmarServiceEntities : DbContext
    {
        public MedmarServiceEntities()
            : base("name=MedmarServiceEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<AcceptBookingRequest> AcceptBookingRequest { get; set; }
        public DbSet<agent> agent { get; set; }
        public DbSet<AgentRequ> AgentRequ { get; set; }
        public DbSet<BookingResponse> BookingResponse { get; set; }
        public DbSet<BookingResponsetType> BookingResponsetType { get; set; }
        public DbSet<BookingStatusHC> BookingStatusHC { get; set; }
        public DbSet<BookingStatusTA> BookingStatusTA { get; set; }
        public DbSet<CancelBookingRequest> CancelBookingRequest { get; set; }
        public DbSet<CancelBookingRequirementRequest> CancelBookingRequirementRequest { get; set; }
        public DbSet<CommentLog> CommentLog { get; set; }
        public DbSet<CommentLogUsers> CommentLogUsers { get; set; }
        public DbSet<ConfirmBookingRequest> ConfirmBookingRequest { get; set; }
        public DbSet<Consumer> Consumer { get; set; }
        public DbSet<Defaults_> Defaults_ { get; set; }
        public DbSet<ETicket> ETicket { get; set; }
        public DbSet<EventData> EventData { get; set; }
        public DbSet<Flight> Flight { get; set; }
        public DbSet<ListOfColumns> ListOfColumns { get; set; }
        public DbSet<ListOfTables> ListOfTables { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<Procedures> Procedures { get; set; }
        public DbSet<requ_SendBookingRequirementRequest> requ_SendBookingRequirementRequest { get; set; }
        public DbSet<requ_SendBookingRequirementRequest_history> requ_SendBookingRequirementRequest_history { get; set; }
        public DbSet<RequireDecisionRequest> RequireDecisionRequest { get; set; }
        public DbSet<RequirementResponse> RequirementResponse { get; set; }
        public DbSet<RequirementResponseStatus> RequirementResponseStatus { get; set; }
        public DbSet<RequireTicketsRequest> RequireTicketsRequest { get; set; }
        public DbSet<resp_SendAvailableBookingRequest> resp_SendAvailableBookingRequest { get; set; }
        public DbSet<SendAvailableBookingRequestFlight> SendAvailableBookingRequestFlight { get; set; }
        public DbSet<SendEticketInfoRequest> SendEticketInfoRequest { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<ServiceStatus> ServiceStatus { get; set; }
        public DbSet<TestRequest> TestRequest { get; set; }
        public DbSet<UpdateFlightDataRequest> UpdateFlightDataRequest { get; set; }
        public DbSet<UpdateFlightsFlight> UpdateFlightsFlight { get; set; }
    }
}
