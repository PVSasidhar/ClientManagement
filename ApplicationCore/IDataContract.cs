using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore
{
    public interface IDataContract<T> : IDisposable
    {
 
            public T Create(T _object);
            public void Delete(T _object);
            public void Update(T _object);
            public IEnumerable<T> GetAll();
            public T GetById(long Id);
       
    }
}
