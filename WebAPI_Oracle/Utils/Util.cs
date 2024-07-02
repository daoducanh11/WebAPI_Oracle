using Oracle.ManagedDataAccess.Client;

namespace WebAPI_Oracle.Utils
{
    public static class Util
    {
        public static OracleDbType GetOracleDbType(object value)
        {
            // Xác định kiểu dữ liệu của giá trị value
            if (value is string)
            {
                return OracleDbType.Varchar2;
            }
            else if (value is int)
            {
                return OracleDbType.Int32;
            }
            else if (value is DateTime || value is DateTime?)
            {
                return OracleDbType.Date;
            }
            else if (value is decimal)
            {
                return OracleDbType.Decimal;
            }
            // Thêm các kiểu dữ liệu khác cần thiết tại đây

            // Mặc định, trả về kiểu dữ liệu nvarchar2 cho các trường hợp không xác định
            return OracleDbType.NVarchar2;
        }
    }
}
