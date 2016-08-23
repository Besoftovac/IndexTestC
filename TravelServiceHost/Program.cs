using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Data.SqlClient;

namespace TravelServiceHost
{
    class Program
    {
        static void Main()
        {
           // SqlConnection conn = TravelService.GeneralSql.CatchDatabase();
            Uri baseAdr = new Uri("http://localhost:8733/");
            using (ServiceHost host = new ServiceHost(typeof(TravelService.TravelService), baseAdr)) {
                host.Open();
                Console.WriteLine("Host is Open");
                Console.ReadLine();
               


            }

        }
    }
}
