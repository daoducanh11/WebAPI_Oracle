using Dapper;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System.Text.Json;
using WebAPI_Oracle.Entities;
using WebAPI_Oracle.Interfaces;
using WebAPI_Oracle.Models;
using WebAPI_Oracle.Utils;
using static Dapper.SqlMapper;

namespace WebAPI_Oracle.Repositories
{
    public class EmployeeRepository : BaseRepository, IEmployeeRepository
    {
        public EmployeeRepository(IConfiguration configuration) : base(configuration)
        {
            
        }

        public async Task<object> GetEmployeeList()
        {
            object result = null;
            try
            {
                var dyParam = new OracleDynamicParameters();
                dyParam.Add("EMPCURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

                string query = "USP_GETEMPLOYEES";
                result = await _sqlConnection.QueryAsync(query, param: dyParam, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<Employee> GetByID(int id)
        {
            var sqlCommand = $"SELECT * FROM Employees WHERE EMPLOYEE_ID = {id}";
            Employee entities = await _sqlConnection.QueryFirstOrDefaultAsync<Employee>(sqlCommand);
            return entities;
        }


        public async Task<PaginationRespone<Employee>> GetByFilter(int currentPage, int pageSize, Object filterEntity)
        {
            var sqlCommand = "Proc_GetEmployees";
            var dyParam = new OracleDynamicParameters();
            Type t = filterEntity.GetType();
            foreach (var prop in t.GetProperties())
            {
                var value = prop.GetValue(filterEntity);
                dyParam.Add($"v_{prop.Name}", Util.GetOracleDbType(value), ParameterDirection.Input, value);
            }
            dyParam.Add("v_currentPage", OracleDbType.Int64, ParameterDirection.Input, currentPage);
            dyParam.Add("v_pageSize", OracleDbType.Int64, ParameterDirection.Input, pageSize);
            dyParam.Add("v_totalRecod", OracleDbType.Int64, ParameterDirection.Output);
            dyParam.Add("v_res", OracleDbType.RefCursor, ParameterDirection.Output);
            var result = await _sqlConnection.QueryAsync<Employee>(sqlCommand, param: dyParam, commandType: CommandType.StoredProcedure);
            PaginationRespone<Employee> paginationRespone = new PaginationRespone<Employee>(currentPage, pageSize);
            int totalRecod = 0;
            int.TryParse(dyParam.Get("v_totalRecod").ToString(), out totalRecod);
            paginationRespone.TotalItem = totalRecod;
            paginationRespone.TotalPage = paginationRespone.TotalItem / pageSize;
            if (paginationRespone.TotalItem % pageSize != 0)
                paginationRespone.TotalPage += 1;
            paginationRespone.Data = result.AsList<Employee>();
            //paginationRespone.Data = (await result.ReadAsync<Employee>()).ToList();
            return paginationRespone;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>ID bản ghi vừa được thêm mới</returns>
        public async Task<decimal> Create(Employee entity)
        {
            var dyParam = new OracleDynamicParameters();
            var properties = entity.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var value = prop.GetValue(entity);
                dyParam.Add($"v_{prop.Name}", Util.GetOracleDbType(value), ParameterDirection.Input, value);
            }
            dyParam.Add("v_ReturnID", OracleDbType.Int64, ParameterDirection.Output);
            var sqlCommand = "Proc_InsertEmployee";
            await _sqlConnection.ExecuteAsync(sqlCommand, param: dyParam, commandType: CommandType.StoredProcedure);
            int empId = 0;
            int.TryParse(dyParam.Get("v_ReturnID").ToString(), out empId);
            return empId;
        }

        public async Task<int> Update(Employee entity)
        {
            var dyParam = new OracleDynamicParameters();
            var properties = entity.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var value = prop.GetValue(entity);
                dyParam.Add($"v_{prop.Name}", Util.GetOracleDbType(value), ParameterDirection.Input, value);
            }
            dyParam.Add("v_ReturnID", OracleDbType.Int64, ParameterDirection.Output);
            var sqlCommand = "PROC_UPDATEEMPLOYEE";
            await _sqlConnection.ExecuteAsync(sqlCommand, param: dyParam, commandType: CommandType.StoredProcedure);
            int rows_updated = 0;
            int.TryParse(dyParam.Get("v_ReturnID").ToString(), out rows_updated);
            return rows_updated;
        }

        public async Task<int> Delete(int empId)
        {
            var sqlCommand = $"DELETE Employees WHERE EMPLOYEE_ID = {empId}";
            return await _sqlConnection.ExecuteAsync(sqlCommand);
        }

        public void WriteFile(EmployeeTmp entity)
        {
            string jsonString = JsonSerializer.Serialize(entity);
            // Đường dẫn đến file văn bản để ghi
            string filePath = @"D:\Downloads\EmployeeTmp.txt";
            try
            {
                // Ghi chuỗi JSON vào file
                File.WriteAllText(filePath, jsonString);

                Console.WriteLine("Đã ghi đối tượng vào file thành công.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Lỗi khi ghi file: {e.Message}");
            }
        }
    }
}
