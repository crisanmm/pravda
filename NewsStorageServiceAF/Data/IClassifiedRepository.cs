using System.Collections.Generic;
using NewsStorage.Entities;

namespace NewsStorage.Data
{
    public interface IClassifiedRepository
    {
        CachedClassified Create(CachedClassified classified);
        IEnumerable<CachedClassified> GetAll();
        CachedClassified GetById(int id);
        void Remove(int id);
        CachedClassified UpdateCache(CachedClassified classified);
    }
}
