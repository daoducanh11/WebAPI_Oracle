using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System;
using WebAPI_Oracle.Entities;
using WebAPI_Oracle.Interfaces;
using WebAPI_Oracle.Models;
using static Dapper.SqlMapper;

namespace WebAPI_Oracle.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        IEmployeeRepository _employeeRepository;
        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        //[Route("api/GetEmployeeList")]
        [HttpGet]
        public async Task<ActionResult> GetEmployeeList()
        {
            var result = await _employeeRepository.GetEmployeeList();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("{empId}")]
        public async Task<ActionResult> GetById(int empId)
        {
            var result = await _employeeRepository.GetByID(empId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost("GetByFilter")]
        public async Task<PaginationRespone<Employee>> GetByFilter([FromBody] FilterEmployee filterEntity, [FromHeader] int currentPage = 1, [FromHeader] int pageSize = 100)
        {
            return await _employeeRepository.GetByFilter(currentPage, pageSize, filterEntity);
        }

        // POST api/<BaseController>
        [HttpPost]
        public async Task<IActionResult> Post(Employee entity)
        {
            try
            {
                Employee emp = await _employeeRepository.GetByID(entity.EMPLOYEE_ID);
                if(emp != null)
                    return BadRequest("Id nhân viên đã tồn tại");
                var res = await _employeeRepository.Create(entity);
                if (res > 0)
                    return StatusCode(201, res);
                return BadRequest(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<BaseController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Employee value)
        {
            try
            {
                if(await _employeeRepository.GetByID(id) == null)
                    return BadRequest("Không tìm thấy nhân viên với id = " + id);
                value.EMPLOYEE_ID = id;
                var res = await _employeeRepository.Update(value);
                if (res > 0)
                    return StatusCode(201, res);
                return BadRequest(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<BaseController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var res = await _employeeRepository.Delete(id);
                if (res > 0)
                    return StatusCode(200, res);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("Children")]
        public IActionResult PostChildren([FromBody] EmployeeTmp entity)
        {
            try
            {                
                _employeeRepository.WriteFile(entity);
                return StatusCode(201, 1);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
