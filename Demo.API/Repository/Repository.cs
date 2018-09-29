using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Demo.API.Data;
using Demo.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Demo.API.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DemoContext _context;
        private DbSet<T> entities;
        public Repository(DemoContext context)
        {
            _context = context;
            entities = _context.Set<T>();
        }

        #region Normal Function
        public IEnumerable<T> GetAll(){
            return entities.AsEnumerable();
        }
        public T GetById(object id){
            return entities.Find(id);
        }
        public void Insert(T entity){
            if (entity == null) {  
                throw new ArgumentNullException("entity");  
            }
            entities.Add(entity);
        }
        public void Delete(T entity){
            if (entity == null) {  
                throw new ArgumentNullException("entity");  
            }
            entities.Remove(entity);
        }
        public void Update(T entity, object key){
            if (entity == null) {  
                throw new ArgumentNullException("entity");  
            }
            T exist = entities.Find(key);
            if (exist != null) {
                _context.Entry(exist).CurrentValues.SetValues(entity);
            }
            else {
                throw new ArgumentNullException("entity");
            }
        }
        #endregion

        #region Async Function
        public async Task<IEnumerable<T>> GetAllAsync(){
            return await entities.ToListAsync();
        }
        public async Task<T> GetByIdAsync(object id){
            return await entities.FindAsync(id);
        }

        //Find Records base on conditions
        public async Task<T> FindAsync(Expression<Func<T, bool>> match){
            return await entities.SingleOrDefaultAsync(match);
        }
        public async Task<ICollection<T>> FindAllAsync(Expression<Func<T,bool>> match){
            return await entities.Where(match).ToListAsync();
        }

        public async Task<T> InsertAsync(T entity){
            if (entity == null) {  
                throw new ArgumentNullException("entity");  
            }
            entities.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<int> DeleteAsync(T entity){
            if (entity == null) {  
                throw new ArgumentNullException("entity");  
            }
            entities.Remove(entity);
            return await _context.SaveChangesAsync();
        }
        public async Task<T> UpdateAsync(T entity, object id){
            var exist = entities.Find(id);
            if (exist != null)
            {
                _context.Entry(exist).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
                return exist;
            }
            else
            {
                throw new ArgumentNullException("entity");
            }
        }
        #endregion

        public void SaveChanges(){
            _context.SaveChanges();
        }
    }
}