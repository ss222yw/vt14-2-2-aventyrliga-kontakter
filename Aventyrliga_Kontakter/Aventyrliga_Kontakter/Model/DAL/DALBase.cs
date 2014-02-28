using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Aventyrliga_Kontakter.Model.DAL
{
    public abstract class DALBase
    {
        //Ett statiskt fält.
        private static string _connectionString;

        //Referens till ett nytt SqlConnection.
        protected SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
        // Initierar statiskt data.
        static DALBase()
        {
            // Hämtar anslutningssträngen från web.config.
            _connectionString = WebConfigurationManager.ConnectionStrings["ContctConnectionString"].ConnectionString;
        }
    }
}