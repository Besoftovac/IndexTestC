using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace TravelServiceHost
{
    class Program
    {
        static void Main()
        {
            Uri baseAdr = new Uri("http://localhost:8733/");
            using (ServiceHost host = new ServiceHost(typeof(TravelService.TravelService), baseAdr)) {
                host.Open();
                Console.WriteLine("Host is Open");
                Console.ReadLine();


            }

        }
    }
}
