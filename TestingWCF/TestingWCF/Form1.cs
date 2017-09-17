using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Services.Client;
using TestingWCF.TravelService;
using System.Runtime.Serialization;

namespace TestingWCF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //public static string ToEnumString<T>(T instance)
        //{
        //    if (!typeof(T).IsEnum)
        //        throw new ArgumentException("instance", "Must be enum type");
        //    string enumString = instance.ToString();
        //    var field = typeof(T).GetField(enumString);
        //    if (field != null) // instance can be a number that was cast to T, instead of a named value, or could be a combination of flags instead of a single value
        //    {
        //        var attr = (EnumMemberAttribute)field.GetCustomAttributes(typeof(EnumMemberAttribute), false).SingleOrDefault();
        //        if (attr != null) // if there's no EnumMember attr, use the default value
        //            enumString = attr.Value;
        //    }
        //    return enumString;
        //}

        private void btnService_Click(object sender, EventArgs e)
        {
            TravelServiceClient tn = new TravelServiceClient();

            var rezult = tn.synchronize(new WebServiceConsumerRequest()

            {
                SessionId = 777778,
                Username = "Offen",
                Password = "R6pRDrQ0pLgZ9Ms",

                //UpdateFlightDataResponses= new BookingResponse[3] {
                //    new BookingResponse {
                //        BookingId=148,
                //        Comment="Test 148!",
                //        IsReceived = true
                //    },
                //     new BookingResponse {
                //         BookingId=156,
                //        Comment="Test 156!",
                //        IsReceived = true
                //     },
                //      new BookingResponse {
                //          BookingId=122,
                //        Comment="Test 122!",
                //        IsReceived = true
                //      }

                //},
                SendBookingRequirementRequests =
                    new SendBookingRequirementRequest[1]

                    {
                        new SendBookingRequirementRequest
                        {

                            BookingRequirementId = 335668,
                             Person = new Person()

                             {

                                 FirstName = "Marinko",
                                 MiddleName = "Zvonko",
                                 LastName = "Perić",
                                // Birthday = new DateTime (1990,02,23)


                                 //Some other properties for person



                             },

                             FromAirport = "HAM",
                             ToAirport = "CPT",
                             ArrivalDate = new DateTime(2017,08,15),
                             RequestBookingComment = "Besoft n-to testiranje servisa koji radi cijelo vrijeme!!!! ",
                             ArrivalTimeRange = TimeRange.AnyTime,
                             DepartureTimeRange = TimeRange.Before8am,
                             IsEmergencyRequest = true,
                             Class = FlightClass.SB

                        } }

                //         new SendBookingRequirementRequest
                //        {

                //            BookingRequirementId = 335667,
                //             Person = new Person()

                //             {

                //                 FirstName = "Ivica",
                //                //MiddleName = "Zvonko",
                //                 LastName = "Ivaniš",
                //                 Birthday = new DateTime (1990,02,23)


                //                 //Some other properties for person



                //             },

                //             FromAirport = "HAM",
                //             ToAirport = "CPT",
                //             ArrivalDate = new DateTime(2017,08,15),
                //             RequestBookingComment = "Besoft - testiranje, ignorirajte ",
                //             ArrivalTimeRange = TimeRange.AnyTime,
                //             DepartureTimeRange = TimeRange.Before8am,
                //             IsEmergencyRequest = true,
                //             Class = FlightClass.SB

                //        }

                //    },

                //CancelBookingRequirementRequests =
                //    new CancelBookingRequirementRequest[1]

                //    {
                //        new CancelBookingRequirementRequest
                //        {

                //            BookingRequirementId = 11614,
                //            Reason="Besoft testiranje - već je otkazan",
                //            Comment = "310517"


                //        }

                //    },

                //AcceptBookingRequests = new AcceptBookingRequest[1] {

                //            new AcceptBookingRequest {
                //                BookingId=127
                //            }
                //        },

                //CancelBookingRequests = new CancelBookingRequest[2] {

                //           new CancelBookingRequest {
                //                BookingId=118,
                //                Comment = "Another booking is choosen ",
                //                BookingStatus= BookingStatus.CANCELLED
                //            },
                //            new CancelBookingRequest {
                //                BookingId=119,
                //                Comment = "Rebooking",
                //                BookingStatus= BookingStatus.CANCELLED
                //            }
                //        },

                //RequireTicketsRequests = new RequireTicketsRequest[3] {

                //         new RequireTicketsRequest {
                //            BookingId=33,
                //            Comment = "test"
                //        },
                //          new RequireTicketsRequest {
                //            BookingId=35,
                //            Comment = "test"
                //        },
                //        new RequireTicketsRequest {
                //            BookingId=36,
                //            Comment = "test"
                //        }

                //    }
                //CancelBookingResponses = new BookingResponse[2] {
                //          new BookingResponse {
                //            BookingId=517,
                //            Comment = "Njihov inicijalni response na naš cancel za id 777",
                //            IsReceived=true
                //        },
                //         new BookingResponse {
                //            BookingId=555,
                //            Comment = "Njihov inicijalni response na naš cancel za id 555",
                //            IsReceived=true
                //        }
                //    }

            });

            WebServiceProviderResponse wpr = rezult;
          
        }

        
    }
}
