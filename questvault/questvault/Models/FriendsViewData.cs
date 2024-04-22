namespace questvault.Models
{
  /// <summary>
  /// Represents view data for a user's friends.
  /// </summary>
  public class FriendsViewData
  {
    /// <summary>
    /// Gets or sets the user.
    /// </summary>
    public User user;
    /// <summary>
    /// Gets or sets a value indicating whether the user has friends.
    /// </summary>

    public bool friends;
    /// <summary>
    /// Gets or sets a value indicating whether the user has sent a friend request.
    /// </summary>
    public bool RequestSent;
    /// <summary>
    /// Gets or sets a value indicating whether the user has received a friend request.
    /// </summary>
    public bool RequestRecieved;
    /// <summary>
    /// Gets or sets the total number of games.
    /// </summary>
    public int nJogosTotal;
    /// <summary>
    /// Gets or sets the number of games being played.
    /// </summary>
    public int nJogosPlaying;
    /// <summary>
    /// Gets or sets the number of completed games.
    /// </summary>
    public int nJogosComplete;
    /// <summary>
    /// Gets or sets the number of retired games.
    /// </summary>
    public int nJogosRetired;
    /// <summary>
    /// Gets or sets the number of backlogged games.
    /// </summary>
    public int nJogosBacklogged;
    /// <summary>
    /// Gets or sets the number of abandoned games.
    /// </summary>
    public int nJogosAbandoned;
    /// <summary>
    /// Gets or sets the number of games in the wishlist.
    /// </summary>
    public int nJogosWishlist;
    /// <summary>
    /// Gets or sets the average rating.
    /// </summary>
    public double RatingAverage;
    /// <summary>
    /// Gets or sets the list of rating counts.
    /// </summary>
    public List<int> ratingCountList;
  }
}
