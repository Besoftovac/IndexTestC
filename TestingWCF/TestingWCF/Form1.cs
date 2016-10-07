﻿using System;
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
                SessionId = 556677,
                Username= "Offen",
                Password= "R6pRDrQ0pLgZ9Ms",

                SendBookingRequirementRequests =
                new SendBookingRequirementRequest[2]

                {
                    new SendBookingRequirementRequest
                    {

                        BookingRequirementId = 123,
                         Person = new Person()

                         {

                             FirstName = "Peter",                           

                             //Some other properties for person

                 

                         },

                         FromAirport = "HAM",
                         ToAirport = "CPT--",
                         ArrivalDate = new DateTime(2016,12,12),
                         RequestBookingComment = "Oni traže booking za Petera - naš je requ response"

                    },
                     new SendBookingRequirementRequest
                    {

                        BookingRequirementId = 106,
                         Person = new Person()

                         {

                             FirstName = "Marinko",                           

                             //Some other properties for person

                 

                         },

                         FromAirport = "HAM---",
                         ToAirport = "CPT",
                         ArrivalDate = new DateTime(2016, 12,11),
                         RequestBookingComment = "Oni traže booking  booking za Marinka - naš je requ response"

                    }

                },

                CancelBookingRequirementRequests =
                new CancelBookingRequirementRequest[1]

                {
                    new CancelBookingRequirementRequest
                    {

                        BookingRequirementId = 253,
                        Reason="",
                        Comment = "Njihov CancelBookingRequirementRequest - naš je requ response"


                    }

                },

                AcceptBookingRequests = new AcceptBookingRequest[3] {

                    new AcceptBookingRequest {
                        BookingId=256,
                        Comment = "AcceptBookingRequest oni šalju - response je naš-commentLogTest(515)"
                    },
                   new AcceptBookingRequest {
                        BookingId=257,
                        Comment = "AcceptBookingRequest oni šalju - response je naš-commentLogTest(516)"
                    },
                   new AcceptBookingRequest {
                        BookingId=258,
                        Comment = "AcceptBookingRequest oni šalju - response je naš-commentLogTest(517)"
                    }
                },

                CancelBookingRequests = new CancelBookingRequest[1] {

                   new CancelBookingRequest {
                        BookingId=517,
                        Comment = "CancelBookingRequest oni šalju - response je naš",
                        BookingStatus= BookingStatus.CANCELLED
                    }
                },
                RequireTicketsRequests = new RequireTicketsRequest[1] {

                     new RequireTicketsRequest {
                        BookingId=555,
                        Comment = "RequireTicketsRequest oni šalju - response je naš"
                    }

                },
                CancelBookingResponses = new BookingResponse[2] {
                      new BookingResponse {
                        BookingId=517,
                        Comment = "Njihov inicijalni response na naš cancel za id 777"
                    },
                     new BookingResponse {
                        BookingId=555,
                        Comment = "Njihov inicijalni response na naš cancel za id 555"
                    }
                }

            });
          
        }

        
    }
}
