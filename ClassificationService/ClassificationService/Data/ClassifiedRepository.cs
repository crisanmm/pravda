using System.Collections.Generic;
using System.Linq;
using ClassificationService.Entities;

namespace ClassificationService.Data
{
    public class ClassifiedRepository : IClassifiedRepository
    {
        private readonly DataContext context;

        public ClassifiedRepository(DataContext context)
        {
            this.context = context;
        }
        public void Create(Classified classified)
        {
            this.context.Add(classified);
            this.context.SaveChanges();
        }

        public void Remove(int id)
        {
            this.context.Remove(this.context.Classifieds.Find(id));
            this.context.SaveChanges();
        }

        public IEnumerable<Classified> GetAll()
        {
            return this.context.Classifieds.ToList();
        }

        public Classified GetById(int id)
        {
            return this.context.Classifieds.Find(id);
        }
    }
}
