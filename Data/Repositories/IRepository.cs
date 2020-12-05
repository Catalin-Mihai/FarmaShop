using System.Collections;
using System.Collections.Generic;

//Crud generic repository interface
namespace FarmaShop.Data.Repositories
{
    public interface IRepository<TEntity>
    {
        IEnumerable<TEntity> GetAll();
        void Add(TEntity entityToCreate); //C.
        TEntity GetById(object id);  //R. Generic id because the id can be integer, string, etc...
        void Update(TEntity entityToDelete); //U.  
        void Delete(TEntity entityToUpdate); //D. // Just mark the entity as 'dirty'

        void Delete(object id); //Delete by id;

        /*IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, 
            string includeProperties = "");*/ // Might be very useful for dynamic filtering, ordering etc... 
    }
}