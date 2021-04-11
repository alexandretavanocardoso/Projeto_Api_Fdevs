using Api.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Api.Data.Context
{
    public class DbContext : IDbContext
    {
        readonly IConfiguration _config;
        public IDbConnection Connection { get; set; }

        public DbContext(IConfiguration config)
        {
            _config = config;
            Connection = new SqlConnection(_config.GetConnectionString("ConnectionString"));
            Connection.Open();
        }

        public void Dispose()
        {
            if (Connection.State != ConnectionState.Closed)
                Connection.Close();
        }
    }
}
