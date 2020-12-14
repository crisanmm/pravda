using System;
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
        public Classified Create(Classified classified)
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

        public IEnumerable<Classified> GetAll()
        {
            return this.context.Classifieds.ToList();
        }

        public Classified GetById(int id)
        {
            return this.context.Classifieds.Find(id);
        }

        public Classified CheckExistence(Classified classified)
        {
            List<Classified> classifieds = this.GetAll().ToList();


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
