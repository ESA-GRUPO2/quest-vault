using questvault.Models;
namespace questvault.Services
{
    /// <summary>
    /// Service for interacting with the IGDB API.
    /// </summary>
    public interface IServiceIGDB
    {
        /// <summary>
        /// Searches for games based on a search term.
        /// </summary>
        /// <param name="searchTerm">The term to search for.</param>
        /// <returns>A collection of games matching the search term sorted by rating.</returns>
        Task<IEnumerable<Game>> SearchGames(string searchTerm);

        /// <summary>
        /// Gets a collection of popular games based on a specified limit.
        /// </summary>
        /// <param name="limit">The maximum number of popular games to retrieve.</param>
        /// <returns>A collection of popular game models.</returns>
        Task<IEnumerable<Game>> GetPopularGames(int limit);

        /// <summary>
        /// Gets a collection of genres.
        /// </summary>
        /// <returns>A collection of genre models.</returns>
        Task<IEnumerable<Genre>> GetGenres();
        /// <summary>
        /// Gets a collection of companies.
        /// </summary>
        /// <returns>A collection of company models.</returns>
        Task<IEnumerable<Company>> GetCompanies();
        Task<IEnumerable<Company>> GetCompaniesFromIds(List<long> ids);
        /// <summary>
        /// Gets a collection of platforms.
        /// </summary>
        /// <returns>A collection of platform models.</returns>
        Task<IEnumerable<Platform>> GetPlatforms();
        /// <summary>
        /// Retrieves platforms from their IDs.
        /// </summary>
        /// <param name="ids">The list of platform IDs.</param>
        /// <returns>The list of platforms corresponding to the provided IDs.</returns>
        Task<IEnumerable<Platform>> GetPlatformsFromIds(List<long> ids);

        Task<IEnumerable<Genre>> GetGenresFromIds(List<long> ids);
    }
}
