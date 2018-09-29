using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Demo.API.Interfaces
{
    public interface IRepository<T> where T : class
    {
         IEnumerable<T> GetAll();
         T GetById(object id);
         void Insert(T entity);
         void Delete(T entity);
         void Update(T entity, object key);

         Task<IEnumerable<T>> GetAllAsync();
         Task<T> GetByIdAsync(object id);

         //Find Record base on conditions
         Task<T> FindAsync(Expression<Func<T, bool>> match);
         Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match);

         Task<T> InsertAsync(T entity);
         Task<T> UpdateAsync(T entity, object key);
         Task<int> DeleteAsync(T entity);
    }
}