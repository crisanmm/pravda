using System.Collections.Generic;
using ClassificationService.Entities;

namespace ClassificationService.Data
{
    public interface IClassifiedRepository
    {
        void Create(Classified classified);
        IEnumerable<Classified> GetAll();
        Classified GetById(int id);
        void Remove(int id);
    }
}
