using System;
using System.Collections.Generic;
using System.Linq;
using NewsStorage.Entities;

namespace NewsStorage.Data
{
    public class CachedClassifiedRepository : IClassifiedRepository
    {
        private readonly DataContext context;

        public CachedClassifiedRepository(DataContext context)
        {
            this.context = context;
        }
        public CachedClassified Create(CachedClassified classified)
        {
            this.context.Add(classified);
            this.context.SaveChanges();
            return classified;
        }

        public void Remove(int id)
        {
            this.context.Remove(this.context.Classifieds.Find(id));
            this.context.SaveChanges();
        }

        public IEnumerable<CachedClassified> GetAll()
        {
            return this.context.Classifieds.ToList();
        }

        public CachedClassified GetById(int id)
        {
            return this.context.Classifieds.Find(id);
        }

        public CachedClassified UpdateCache(CachedClassified classified)
        {
            List<CachedClassified> classifieds = this.GetAll().ToList();

            var temp = classifieds.Find(c => c.Title == classified.Title && c.Date == classified.Date);

            if (temp != null)
            {
                classified = temp;

                if ((DateTime.Now - classified.ResetTime).TotalHours < 24)
                {
                    classified.Today++;
                }
                else if ((DateTime.Now - classified.ResetTime).TotalHours < 48)
                {
                    classified.Before_Yesterday = classified.Yesterday;
                    classified.Yesterday = classified.Today;
                    classified.Today = 1;
                    classified.ResetTime = DateTime.Now;
                }
                else if ((DateTime.Now - classified.ResetTime).TotalHours < 72)
                {
                    classified.Before_Yesterday = classified.Today;
                    classified.Yesterday = 0;
                    classified.Today = 1;
                    classified.ResetTime = DateTime.Now;
                }
                else
                {
                    classified.Before_Yesterday = 0;
                    classified.Yesterday = 0;
                    classified.Today = 1;
                    classified.ResetTime = DateTime.Now;
                }


                this.context.Update(classified);
                this.context.SaveChanges();
            }
            else
            {
                classified = null;
            }


            return classified;
        }
        
    }
}
