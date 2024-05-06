using System.Data;
using System.Data.Common;
using AutoMapper;
using AutoMapper.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TestSQLInjection.Server.Extensions;

namespace TestSQLInjection.Server.Extensions
{
    public class CustomTypeSqlQuery<T> where T : class
    {
        private IMapper _mapper;

        public DatabaseFacade DatabaseFacade { get; set; }
        public string SqlQuery { get; set; }
        public DbParameter[] Parameters { get; set; }

        public CustomTypeSqlQuery()
        {
            _mapper = new MapperConfiguration(cfg => {
                cfg.AddDataReaderMapping();
                cfg.CreateMap<IDataRecord, T>();
                // Todo-Api-Merge
                cfg.ShouldMapProperty = pi =>
                        !pi.PropertyType.IsNonStringEnumerable()
                        && pi.PropertyType != typeof(T);
            }).CreateMapper();
        }

        public async Task<List<T>> ToListAsync()
        {
            var results = new List<T>();
            var conn = DatabaseFacade.GetDbConnection();
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    await conn.OpenAsync();
                }

                using (var command = conn.CreateCommand())
                {
                    command.CommandText = SqlQuery;
                    command.CommandTimeout = DatabaseFacade.GetCommandTimeout() ?? 120;

                    if (Parameters?.Length > 0)
                    {
                        command.Parameters.AddRange(Parameters);
                    }

                    var reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                        results = _mapper.Map<IDataReader, IEnumerable<T>>(reader)
                            .ToList();
                    reader.Dispose();
                }
            }
            finally
            {
                conn.Close();
            }
            return results;
        }

        public async Task<T> FirstOrDefaultAsync()
        {
            T result = null;
            var conn = DatabaseFacade.GetDbConnection();
            try
            {
                await conn.OpenAsync();
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = SqlQuery;

                    if (Parameters?.Length > 0)
                    {
                        command.Parameters.AddRange(Parameters);
                    }

                    var reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        var results = _mapper.Map<IDataReader, IEnumerable<T>>(reader)
                            .ToList();
                        result = results.FirstOrDefault();
                    }
                    reader.Dispose();
                }
            }
            finally
            {
                conn.Close();
            }
            return result;
        }
    }
    
}
