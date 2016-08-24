using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace TravelService
{
    class StoredProceduresCall
    {

        static SqlConnection conn = GeneralSql.CatchDatabase();

        private static SqlCommand InitSqlCommand(String strProcedureName /*SqlConnection conn*/)
        {

            SqlCommand cmd = new SqlCommand(strProcedureName, conn);
            cmd.CommandType = CommandType.StoredProcedure;
           // cmd.Parameters.Add("@Verzija", SqlDbType.Int).Value = (int)verzija_programa.Jedan;

            return cmd;
        }

        private static void Execute(SqlCommand cmd, bool bCloseConn = true)
        {
            if (conn != null && conn.State == ConnectionState.Closed)
                conn.Open();
            cmd.ExecuteNonQuery();
            if (bCloseConn) conn.Close();
        }
    }
}
