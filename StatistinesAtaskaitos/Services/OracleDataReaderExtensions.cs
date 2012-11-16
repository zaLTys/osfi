using System;
using Oracle.DataAccess.Client;

namespace StatistinesAtaskaitos.Services
{
    public static class OracleDataReaderExtensions
    {
        public static TResult ReadValue<TResult>(this OracleDataReader reader, string column)
        {
            var value = reader[column];
            var resultType = typeof (TResult);
            return (TResult)Convert.ChangeType(value, resultType);
        }

        public static TResult? ReadNullableValue<TResult>(this OracleDataReader reader, string column)
            where TResult : struct 
        {
            var value = reader[column];
            var resultType = typeof(TResult);

            if (value == DBNull.Value) return null;
            return (TResult)Convert.ChangeType(value, resultType);
        }
    }
}