using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FarmaShop.Data.DAL;
using Microsoft.EntityFrameworkCore;

namespace FarmaShop.Data.Repositories
{
    
    //Generic repository that implements the generic repository interface
    public class BaseRepository<TEntity>: IRepository<TEntity> where TEntity : class
    {
        protected ApplicationDbContext _context;
        protected DbSet<TEntity> _dbSet; //TEntity must be a class! Constraint added for the class generic argument
        
        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }
        
        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<TEntity> GetById(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task Add(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task Delete(object id)
        {
            var entityToDelete = await _dbSet.FindAsync(id);
            Delete(entityToDelete);
        }

        public async Task<IEnumerable<TEntity>> Get(
            Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, 
            string includeProperties = "")
        {
            //Start with querying the whole table 
            IQueryable<TEntity> query = _dbSet;

            //Filter result (if any filter used)
            if (filter != null) {
                query = query.Where(filter);
            }

            //Include properties (joins with another tables)
            if (includeProperties != null) {
                foreach (var includeProperty in includeProperties.Split
                    (new char[] {','}, StringSplitOptions.RemoveEmptyEntries)) {
                    query = query.Include(includeProperty);
                }
            }
            
            //Order result (if any ordering is specified)
            if (orderBy != null) {
                var res = await orderBy(query).ToListAsync();
                return res;
            }

            //Return result
            return await query.ToListAsync();
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            //Verify if the entity was queried with .AsNoTracking()
            //If it is non-tracked then track it to make EF know about it.
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void DetachEntity(TEntity entityToDetach)
        {
            _context.Entry(entityToDetach).State = EntityState.Detached;
        }
    }
}