using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

//Crud generic repository interface
namespace FarmaShop.Data.Repositories
{
    public interface IRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task Add(TEntity entityToCreate); //C.
        public Task AddRange(IEnumerable<TEntity> entities); //C. Creates range
        Task<TEntity> GetById(object id);  //R. Generic id because the id can be integer, string, etc...
        void Update(TEntity entityToDelete); //U.  
        void Delete(TEntity entityToUpdate); //D.

        void DeleteRange(IEnumerable<TEntity> entities); //D. Deletes range
        Task Delete(object id); //Delete by id;

        Task<IEnumerable<TEntity>> Get(
            Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, 
            string includeProperties = ""); // Might be very useful for dynamic filtering, ordering etc... 

        Task SaveChangesAsync();

        void DetachEntity(TEntity entityToDetach);
    }
}