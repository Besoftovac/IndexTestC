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
                         ToAirport = "CPT",
                         ArrivalDate = new DateTime(2016,12,12),
                         RequestBookingComment = "First test booking request!"

                    },
                     new SendBookingRequirementRequest
                    {

                        BookingRequirementId = 106,
                         Person = new Person()

                         {

                             FirstName = "Marinko",                           

                             //Some other properties for person

                 

                         },

                         FromAirport = "HAM",
                         ToAirport = "CPT",
                         ArrivalDate = new DateTime(2016, 12,11),
                         RequestBookingComment = "Test booking request!"

                    }

                },

                CancelBookingRequirementRequests =
                new CancelBookingRequirementRequest[1]

                {
                    new CancelBookingRequirementRequest
                    {

                        BookingRequirementId = 253,
                        Reason="",
                        Comment = ""


                    }

                },

                AcceptBookingRequests = new AcceptBookingRequest[1] {

                    new AcceptBookingRequest {
                        BookingId=556,
                        Comment = "Test AcceptBookingRequest"
                    }
                },

                CancelBookingRequests = new CancelBookingRequest[1] {

                   new CancelBookingRequest {
                        BookingId=555,
                        Comment = "Test CancelBookingRequest",
                        BookingStatus= BookingStatus.CANCELLED
                    }
                },
                RequireTicketsRequests = new RequireTicketsRequest[1] {

                     new RequireTicketsRequest {
                        BookingId=555,
                        Comment = "Test RequireTicketsRequest"
                    }

                }
            });
          
        }

        
    }
}
