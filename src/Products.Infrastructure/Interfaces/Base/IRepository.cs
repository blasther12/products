using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Products.Infrastructure.Interfaces.Base
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Register(TEntity entity);
        void Delete(int id);
        void Update(TEntity entity);
        List<TEntity> ListRecords();
        TEntity GetById(int id);
    }
}