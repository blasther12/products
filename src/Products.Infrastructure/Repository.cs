using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities.Base;
using Products.Infrastructure.ExceptionHandling;
using Products.Infrastructure.Interfaces.Base;

namespace Products.Infrastructure
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        protected DbContext Db;
        protected DbSet<TEntity> DbSetContext;

        public Repository(DbContext dbContext)
        { 
            Db = dbContext;
            DbSetContext = Db.Set<TEntity>();
        }

        public async Task Delete(long id)
        {
            var entity = await GetById(id) ?? throw new NotFoundException("No data found for deletion!");
            Db.Remove(entity);
            await Db.SaveChangesAsync();
        }

        public async Task<TEntity?> GetById(long id)
        {
            return await DbSetContext.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Register(TEntity entity)
        {
            await DbSetContext.AddAsync(entity);
            await Db.SaveChangesAsync();
        }

        public async Task Update(TEntity entity)
        {
            DbSetContext.Attach(entity);
            Db.Entry(entity).State = EntityState.Modified;
            await Db.SaveChangesAsync();
        }

        async Task<List<TEntity>> IRepository<TEntity>.ListRecords()
        {
            return await DbSetContext.ToListAsync();
        }
    }
}