using WebAPI_Oracle.Entities;
using WebAPI_Oracle.Models;

namespace WebAPI_Oracle.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<object> GetEmployeeList();

        Task<Employee> GetByID(int id);

        Task<PaginationRespone<Employee>> GetByFilter(int currentPage, int pageSize, Object filterEntity);

        Task<decimal> Create(Employee entity);

        Task<int> Update(Employee entity);

        Task<int> Delete(int empId);

        void WriteFile(EmployeeTmp entity);
    }
}
