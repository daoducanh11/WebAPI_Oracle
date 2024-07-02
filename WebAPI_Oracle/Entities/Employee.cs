namespace WebAPI_Oracle.Entities
{
    public class Employee
    {
        public int EMPLOYEE_ID { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string EMAIL { get; set; }
        public string PHONE_NUMBER { get; set; }
        public DateTime? HIRE_DATE { get; set; }
        //public string JOB_ID { get; set; }
        public decimal SALARY { get; set; }
        //public int COMMISSION_PCT { get; set; }
        //public int MANAGER_ID { get; set; }
        //public int DEPARTMENT_ID { get; private set; }
    }

    public class EmployeeTmp : Employee
    {
        public List<Children> Childrens { get; set; }
    }

    public class Children
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
