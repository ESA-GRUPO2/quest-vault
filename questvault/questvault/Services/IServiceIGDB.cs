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
        Task<IEnumerable<Models.Game>> SearchGames(string searchTerm);

        /// <summary>
        /// Gets a collection of popular games based on a specified limit.
        /// </summary>
        /// <param name="limit">The maximum number of popular games to retrieve.</param>
        /// <returns>A collection of popular game models.</returns>
        Task<IEnumerable<Models.Game>> GetPopularGames(int limit);

        /// <summary>
        /// Gets a collection of genres.
        /// </summary>
        /// <returns>A collection of genre models.</returns>
        Task<IEnumerable<Models.Genre>> GetGenres();
        /// <summary>
        /// Gets a collection of companies.
        /// </summary>
        /// <returns>A collection of company models.</returns>
        Task<IEnumerable<Models.Company>> GetCompanies();
        /// <summary>
        /// Gets a collection of platforms.
        /// </summary>
        /// <returns>A collection of platform models.</returns>
        Task<IEnumerable<Models.Platform>> GetPlatforms();
    }
}
