using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace TravelService
{
    public class GeneralSql
    {

        public static SqlConnection CatchDatabase(bool local = true) {

            String user = "Korisnik";
            String pass = "bEbE7112";

            String konekcija = null;

            if (local)
            {
                //Data Source=.;Initial Catalog=DATABASE_NAME;Integrated Security=True;
                konekcija = String.Format(@"Initial Catalog=MedmarService; Data Source=.\SQLEXPRESS;Integrated Security=True;");

            }
            else {
                konekcija = String.Format(@"Initial Catalog=MedmarService; Data Source=89.164.66.22;User Id={0};Password={1}", user, pass);
            }           
            
            SqlConnection conn = new SqlConnection(konekcija);
           
            return conn;
              
        }

    }
}
