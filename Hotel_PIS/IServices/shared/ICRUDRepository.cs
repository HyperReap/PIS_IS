using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel_PIS.IServices.Shared
{
    public interface ICRUDRepository<T> where T : class
    {
        public T Save(T obj);
        public void Delete(int id);
        public T Get(int id);
        public List<T> GetAll();
    }
}
