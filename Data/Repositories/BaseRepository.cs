using System.Collections.Generic;
using System.Linq;
using FarmaShop.Data.DAL;
using Microsoft.EntityFrameworkCore;

namespace FarmaShop.Data.Repositories
{
    
    //Generic repository that implements the generic repository interface
    public class BaseRepository<TEntity>: IRepository<TEntity> where TEntity : class
    {
        private ApplicationDbContext _context;
        private DbSet<TEntity> _dbSet; //TEntity must be a class! Constraint added for the class generic argument
        
        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }
        
        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet.ToList();
        }

        public virtual TEntity GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public virtual void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            var entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
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
            // _dbSet.Attach(entityToUpdate); //Not needed?
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}