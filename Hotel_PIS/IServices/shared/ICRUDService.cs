using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel_PIS.IServices.Shared
{
    public interface ICRUDService<T> where T : class
    {
        public T Save(int id, T obj);
        public bool Delete(int id);
        public T Get(int id);
        public List<T> GetAll();
    }
}
