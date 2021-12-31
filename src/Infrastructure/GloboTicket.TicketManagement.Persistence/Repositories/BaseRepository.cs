using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Persistence.Repositories
{
    [ExcludeFromCodeCoverage]
    public class BaseRepository<T> : IAsyncRepository<T> where T : class
    {
        protected readonly GloboTicketDbContext _dbContext;
        private readonly ILogger _logger;
        public BaseRepository(GloboTicketDbContext dbContext, ILogger<T> logger)
        {
            _dbContext = dbContext; _logger = logger;
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            _logger.LogInformation("ListAllAsync Initiated");
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async virtual Task<IReadOnlyList<T>> GetPagedReponseAsync(int page, int size)
        {
            return await _dbContext.Set<T>().Skip((page - 1) * size).Take(size).AsNoTracking().ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        //For Read Operation
        public async Task<IList<T>> StoredProcedureQueryAsync(string storedProcedureName, SqlParameter[] parameters = null)
        {
            var parameterNames = GetParameterNames(parameters);
            return await _dbContext.Set<T>().FromSqlRaw(string.Format("{0} {1}", storedProcedureName, string.Join(",", parameterNames)), parameters).ToListAsync();
        }

        //For Insert, Update, Delete Operations
        public async Task<int> StoredProcedureCommandAsync(string storedProcedureName, SqlParameter[] parameters = null)
        {
            var parameterNames = GetParameterNames(parameters);
            return await _dbContext.Database.ExecuteSqlRawAsync(string.Format("{0} {1}", storedProcedureName, string.Join(",", parameterNames)), parameters);
        }

        private string[] GetParameterNames(SqlParameter[] parameters)
        {
            var parameterNames = new string[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                parameterNames[i] = parameters[i].ParameterName;
            }
            return parameterNames;
        }
    }
}
