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
                SessionId = 5050,
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
                //SendBookingRequirementRequests =
                //    new SendBookingRequirementRequest[3]

                //    {
                //        new SendBookingRequirementRequest
                //        {

                //            BookingRequirementId = 100,
                //             Person = new Person()

                //             {

                //                 FirstName = "Peter", 
                //                 MiddleName = "Zvonko"


                //                 //Some other properties for person



                //             },

                //             FromAirport = "HAM",
                //             ToAirport = "CPT",
                //             ArrivalDate = new DateTime(2016,12,12),
                //             RequestBookingComment = "Test comment 100"

                //        },
                //         new SendBookingRequirementRequest
                //        {

                //           // BookingRequirementId = 106,
                //             Person = new Person()

                //             {

                //                 FirstName = "No id",                           

                //                 //Some other properties for person



                //             },

                //             FromAirport = "HAM",
                //             ToAirport = "CPT",
                //             ArrivalDate = new DateTime(2016, 12,11),
                //             RequestBookingComment = "no id test"

                //        },
                //          new SendBookingRequirementRequest
                //        {

                //            BookingRequirementId = 101,
                //             Person = new Person()

                //             {

                //                 FirstName = "Marinko",                           

                //                 //Some other properties for person



                //             },

                //             FromAirport = "HAM---",
                //             ToAirport = "CPT",
                //             ArrivalDate = new DateTime(2016, 12,11),
                //             RequestBookingComment = "Test comment 101"

                //        }

                //    }

                //CancelBookingRequirementRequests =
                //    new CancelBookingRequirementRequest[1]

                //    {
                //        new CancelBookingRequirementRequest
                //        {

                //            BookingRequirementId = 253,
                //            Reason="",
                //            Comment = "Njihov CancelBookingRequirementRequest - naš je requ response"


                //        }

                //    },

                //AcceptBookingRequests = new AcceptBookingRequest[8] {

                //        new AcceptBookingRequest {
                //            BookingId=15016
                //        },
                //       new AcceptBookingRequest {
                //            BookingId=17

                //        },
                //       new AcceptBookingRequest {
                //            BookingId=19,
                //            Comment = "so expensive!!!"


                //        },
                //         new AcceptBookingRequest {
                //            BookingId=15021

                //        },
                //       new AcceptBookingRequest {
                //            BookingId=31

                //        },
                //       new AcceptBookingRequest {
                //            BookingId=33

                //        },
                //        new AcceptBookingRequest {
                //            BookingId=15030

                //        },
                //       new AcceptBookingRequest {
                //            BookingId=37

                //        }
                //    },

                //CancelBookingRequests = new CancelBookingRequest[4] {

                //       new CancelBookingRequest {
                //            BookingId=522,
                //            Comment = "CancelBookingRequest oni šalju - response je naš",
                //            BookingStatus= BookingStatus.CANCELLED
                //        },
                //        new CancelBookingRequest {
                //            BookingId=15030,
                //            Comment = "CancelBookingRequest oni šalju - response je naš",
                //            BookingStatus= BookingStatus.CANCELLED
                //        },
                //         new CancelBookingRequest {
                //            Comment = "no id",
                //            BookingStatus= BookingStatus.CANCELLED
                //        },
                //        new CancelBookingRequest {
                //            BookingId=15021,
                //            Comment = "CancelBookingRequest oni šalju - response je naš",
                //            BookingStatus= BookingStatus.CANCELLED
                //        }
                //    },
                //RequireTicketsRequests = new RequireTicketsRequest[3] {

                //         new RequireTicketsRequest {
                //            BookingId=15038,
                //            Comment = "Zadnji comment u comment logu 15038"
                //        },
                //          new RequireTicketsRequest {
                //            BookingId=15030,
                //            Comment = "Zadnji comment u comment logu 15030"
                //        },
                //        new RequireTicketsRequest {
                //            BookingId=15021,
                //            Comment = "Zadnji comment u comment logu 15021"
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
