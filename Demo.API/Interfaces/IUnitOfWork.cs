using System;

namespace Demo.API.Interfaces
{
    public interface IUnitOfWork<T> : IDisposable where T :class
    {
         IRepository<T> ClassRepository {get;}
         void Save();
    }
}