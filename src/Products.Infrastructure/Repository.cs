using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities.Base;
using Products.Infrastructure.Interfaces.Base;

namespace Products.Infrastructure
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase, new()
    {
        protected DbContext Db;
        protected DbSet<TEntity> DbSetContext;

        public Repository(DbContext dbContext)
        { 
            Db = dbContext;
            DbSetContext = Db.Set<TEntity>();
        }
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public TEntity GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<TEntity> ListRecords()
        {
            throw new NotImplementedException();
        }

        public void Register(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}