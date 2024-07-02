using Dapper;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace WebAPI_Oracle.Utils
{
    public class OracleDynamicParameters : SqlMapper.IDynamicParameters
    {
        private readonly DynamicParameters dynamicParameters = new DynamicParameters();
        private readonly List<OracleParameter> oracleParameters = new List<OracleParameter>();

        public void Add(string name, OracleDbType oracleDbType, ParameterDirection direction, object value = null, int? size = null)
        {
            OracleParameter oracleParameter;
            if (size.HasValue)
            {
                oracleParameter = new OracleParameter(name, oracleDbType, size.Value, value, direction);
            }
            else
            {
                oracleParameter = new OracleParameter(name, oracleDbType, value, direction);
            }

            oracleParameters.Add(oracleParameter);
        }

        public void Add(string name, OracleDbType oracleDbType, ParameterDirection direction)
        {
            var oracleParameter = new OracleParameter(name, oracleDbType, direction);
            oracleParameters.Add(oracleParameter);
        }

        public void AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            ((SqlMapper.IDynamicParameters)dynamicParameters).AddParameters(command, identity);

            var oracleCommand = command as OracleCommand;

            if (oracleCommand != null)
            {
                oracleCommand.Parameters.AddRange(oracleParameters.ToArray());
            }
        }

        /// <summary>
        /// All the names of the param in the bag, use Get to yank them out
        /// </summary>
        public IEnumerable<string> ParameterNames
        {
            get
            {
                return oracleParameters.Select(p => p.ParameterName);
            }
        }

        /// <summary>
        /// Get the value of a parameter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns>The value, note DBNull.Value is not returned, instead the value is returned as null</returns>
        public object Get(string name)
        {
            return oracleParameters.FirstOrDefault(f => f.ParameterName == name).Value;
        }

        //public T Get<T>(string name)
        //{
        //    var paramInfo = GetParameter(name);
        //    var attachedParam = paramInfo.AttachedParam;
        //    object val = attachedParam == null ? paramInfo.Value : attachedParam.Value;
        //    if (val == DBNull.Value)
        //    {
        //        if (default(T) != null)
        //        {
        //            throw new ApplicationException("Attempting to cast a DBNull to a non nullable type! Note that out/return parameters will not have updated values until the data stream completes (after the 'foreach' for Query(..., buffered: false), or after the GridReader has been disposed for QueryMultiple)");
        //        }
        //        return default(T);
        //    }

        //    return OracleValueConverter.Convert<T>(val);
        //}
    }
}
