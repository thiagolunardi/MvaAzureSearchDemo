using System;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace MvaAzureSearchDemo.Data
{
    public abstract class MySqlContext : IDisposable
    {
        protected IDbConnection Connection
        {
            get
            {
                var connectionString = ConfigurationManager.ConnectionStrings["DbContext"].ConnectionString;
                return new MySqlConnection(connectionString);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}