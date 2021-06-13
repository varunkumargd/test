using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CodingChallenge.DataAccess.Models
{

    [DataContract(Name = "Movie")]
    public class Movie
    {
        static readonly List<string>  articles = new List<string>() { "a", "an", "the" };

        [DataMember(Name = "ID", Order = 1)]
        public int ID { get; set; }
        [DataMember(Name = "Title", Order = 2)]
        public string Title { get; set; }
        [DataMember(Name = "Year", Order = 3)]
        public int Year { get; set; }
        [DataMember(Name = "Rating", Order = 4)]
        public double Rating { get; set; }
        [DataMember(Name = "Franchise", Order = 5)]
        public string Franchise { get; set; }
        public string TitleWithoutArticle
        {
            get
            {
                foreach (var article in articles)
                {
                    if (Title.ToLower().StartsWith(article + " "))
                    {
                        return Title.Substring((article.Length + 1), Title.Length - (article.Length + 1));
                    }
                }
                return Title;
            }
        }
    }
}
