using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Products.Infrastructure.Interfaces.Base
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task Register(TEntity entity);
        Task Delete(long id);
        Task Update(TEntity entity);
        Task<List<TEntity>> ListRecords();
        Task<TEntity?> GetById(long id);
    }
}