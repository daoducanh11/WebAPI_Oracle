using Dapper;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using WebAPI_Oracle.Interfaces;
using WebAPI_Oracle.Utils;

namespace WebAPI_Oracle.Repositories
{
    public class BaseRepository : IBaseRepository
    {
        protected readonly string _connectionString;
        protected readonly OracleConnection _sqlConnection;
        public BaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetSection("ConnectionStrings").GetSection("OrclConnection").Value; ;
            _sqlConnection = new OracleConnection(_connectionString);
        }

        
    }
}
