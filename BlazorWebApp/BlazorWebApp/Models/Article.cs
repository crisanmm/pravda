using System;

namespace BlazorWebApp.Models
{
    public class Article
    {
        public Article()
        {
        }

        public Article(string Guid, string Title, string Link, string Description, DateTime PubDate, string Source)
        {
            this.Guid = Guid;
            this.Title = Title;
            this.Link = Link;
            this.Description = Description;
            this.PubDate = PubDate;
            this.Source = Source;
        }

        public string Title { get; set; }

        public string Link { get; set; }

        public string Guid { get; set; }

        public DateTime PubDate { get; set; }

        public string Description { get; set; }

        public string Source { get; set; }
    }
}