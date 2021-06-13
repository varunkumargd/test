using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using CodingChallenge.DataAccess.Interfaces;
using CodingChallenge.DataAccess.Models;
using CodingChallenge.Utilities;

namespace CodingChallenge.DataAccess
{
    public class LibraryService : ILibraryService
    {
        public LibraryService() { }

        private IEnumerable<Movie> GetMovies()
        {

            var movies= _movies ?? (_movies = ConfigurationManager.AppSettings["LibraryPath"].FromFileInExecutingDirectory()
                .DeserializeFromXml<Library>().Movies).OrderBy(s=>s.Title);
            var duplicateEntry = new List<Movie>();

            var previousMovieItem=new Movie();
            foreach (var currentMovieItem in movies)
            {
                if (currentMovieItem.Title == previousMovieItem.Title && previousMovieItem.Year==currentMovieItem.Year && previousMovieItem.Rating==currentMovieItem.Rating)
                {
                    duplicateEntry.Add(currentMovieItem);
                }
                previousMovieItem = currentMovieItem;
            }

            return duplicateEntry.Count() > 0 ? _movies.Except(duplicateEntry) : _movies;
        }
        private IEnumerable<Movie> _movies { get; set; }

        public int SearchMoviesCount(string title)
        {
            return SearchMovies(title).Count();
        }

        public IEnumerable<Movie> SearchMovies(string title, int? skip = null, int? take = null, string sortColumn = null, SortDirection sortDirection = SortDirection.Ascending)
        {
            sortColumn = (sortColumn == "Title") ? "TitleWithoutArticle" : sortColumn;
            var movies = GetMovies().Where(s => s.Title.Contains(title)).Sort(sortColumn, (sortDirection == SortDirection.Ascending ? false : true));
            if (skip.HasValue && take.HasValue)
            {
                movies = movies.Skip(skip.Value).Take(take.Value);
            }

            return movies.ToList();
        }
    }


}
