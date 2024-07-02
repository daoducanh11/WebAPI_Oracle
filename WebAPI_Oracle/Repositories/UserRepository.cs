using Dapper;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Reflection;
using WebAPI_Oracle.Entities;
using WebAPI_Oracle.Interfaces;
using WebAPI_Oracle.Models;
using WebAPI_Oracle.Utils;
using static Dapper.SqlMapper;

namespace WebAPI_Oracle.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<int> CheckLogin(LoginModel model)
        {
            var sqlCommand = $"SELECT * FROM (SELECT UserId FROM Users WHERE UserName = :Username AND Password = :Password) WHERE ROWNUM = 1";
            var dyParam = new OracleDynamicParameters();
            dyParam.Add(":Username", OracleDbType.Varchar2, ParameterDirection.Input, model.Username);
            dyParam.Add(":Password", OracleDbType.Varchar2, ParameterDirection.Input, model.Password);
            return await _sqlConnection.QueryFirstOrDefaultAsync<int>(sqlCommand, param: dyParam, commandType: CommandType.Text);
        }

        public async Task<int> UpdateRefreshToken(User user)
        {
            var sqlCommand = $"UPDATE Users SET RefreshToken = :RefreshToken, RefreshTokenExpiryTime = :RefreshTokenExpiryTime WHERE UserId = :UserId";
            var dyParam = new OracleDynamicParameters();
            dyParam.Add(":RefreshToken", OracleDbType.Varchar2, ParameterDirection.Input, user.RefreshToken);
            dyParam.Add(":RefreshTokenExpiryTime", OracleDbType.Date, ParameterDirection.Input, user.RefreshTokenExpiryTime);
            dyParam.Add(":UserId", OracleDbType.Int64, ParameterDirection.Input, user.UserId);
            int res = await _sqlConnection.ExecuteAsync(sqlCommand, param: dyParam);
            return res;
        }

        public async Task<User> GetByID(int id)
        {
            var sqlCommand = $"SELECT * FROM (SELECT * FROM Users WHERE UserId = {id}) WHERE ROWNUM = 1";
            User entities = await _sqlConnection.QueryFirstOrDefaultAsync<User>(sqlCommand);
            return entities;
        }
    }
}
