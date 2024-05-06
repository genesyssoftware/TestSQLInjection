using System.Data.Common;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TestSQLInjection.Server.Extensions;

namespace TestSQLInjection.Server.Extensions
{
    public static class DatabaseExtensions
    {
        public static CustomTypeSqlQuery<T> SqlQuery<T>(
            this DatabaseFacade database,
            string sqlQuery,
            params DbParameter[] parameters) where T : class
        {
            return new CustomTypeSqlQuery<T>()
            {
                DatabaseFacade = database,
                SqlQuery = sqlQuery,
                Parameters = parameters
            };
        }
    }
}
