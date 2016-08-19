using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Web;
using System.ServiceModel;

namespace Travel
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Uri baseAddress = new Uri("http://localhost:8000/");

            WebServiceHost svcHost = new WebServiceHost(typeof(Travel.sincService));

            try
            {
                svcHost.Open();

                Console.WriteLine("Service is running");
                Console.WriteLine("Press enter to quit...");
                Console.ReadLine();

                svcHost.Close();
            }
            catch (CommunicationException cex)
            {
                Console.WriteLine("An exception occurred: {0}", cex.Message);
                svcHost.Abort();
            }
        }
    }
}