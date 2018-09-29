using Demo.API.Data;
using Demo.API.Interfaces;

namespace Demo.API.Repository
{
    public class UnitOfWork<T> : IUnitOfWork<T> where T : class
    {
        private IRepository<T> _repository;
        private readonly DemoContext _context;
        public UnitOfWork(DemoContext context)
        {
            _context = context;

        }
        public IRepository<T> ClassRepository
        {
            get
            {
                if (_repository == null)
                {
                    _repository = new Repository<T>(_context);
                }
                return _repository;
            }
        }

        public void Save(){
            _context.SaveChanges();
        }
        private bool disposed = false;
 
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
    
        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }
    }
}