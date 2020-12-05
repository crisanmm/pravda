using System.Collections.Generic;
using WebApplication1.Entities;

namespace WebApplication1.Data
{
    public interface IClassifiedRepository
    {
        void Create(Classified classified);
        IEnumerable<Classified> GetAll();
        Classified GetById(int id);
        void Remove(int id);
    }
}
