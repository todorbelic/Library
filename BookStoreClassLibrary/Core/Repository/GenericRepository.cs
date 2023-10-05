using BookStoreClassLibrary.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookStoreClassLibrary.Core.Repository
{
    public class GenericRepository<T, Key> : IGenericRepository<T, Key> where T : class, IEntity<Key>
    {
        private readonly DbContext _context;
        private DbSet<T> _entities;

        public GenericRepository(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _entities = context.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return _entities;
        }

        public async Task<T> GetByIdAsync(Key id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> entities = _context.Set<T>();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    entities = entities.Include(include);
                }
            }
            return await entities.FirstOrDefaultAsync(s => s.Id.Equals(id));
        }

        public async Task<T> Add(T entity)
        {
            await _entities.AddAsync(entity);
            return entity;
        }

        public T Update(T entity)
        {
            _entities.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public void Delete(T entity)
        {
            _entities.Remove(entity);
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }
    }
}
