using System.Collections.Generic;
using ClassificationService.Entities;

namespace ClassificationService.Data
{
    public interface IClassifiedRepository
    {
        Classified Create(Classified classified);
        IEnumerable<Classified> GetAll();
        Classified GetById(int id);
        void Remove(int id);
        Classified CheckExistence(Classified classified);
    }
}
