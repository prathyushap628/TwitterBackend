using Twitter.Models;
using Dapper;
using Twitter.Utilities;
using Twitter.Repositories;
using Tweet.Models;
using Twitter.Models;

namespace Twitter.Repositories;

public interface ITweetRepository
{
    Task<TweetItem> Create(TweetItem Item);
    Task Update(TweetItem Item);
    Task Delete(int Id);
    Task<List<TweetItem>> GetAll();
    Task<TweetItem> GetById(int TweetId);
    Task<List<TweetItem>> GetTweetsByUserId(int UserId);
}

public class TweetRepository : BaseRepository, ITweetRepository
{
    public TweetRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<TweetItem> Create(TweetItem Item)
    {
        var query = $@"INSERT INTO {TableNames.tweet} (title, user_id, created_at, updated_at) 
        VALUES (@Title, @UserId, @CreatedAt, @UpdatedAt) RETURNING *";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<TweetItem>(query, Item);
    }

    public async Task Delete(int TweetId)
    {
        var query = $@"DELETE FROM {TableNames.tweet} WHERE tweet_id = @TweetId";

        using (var con = NewConnection)
            await con.ExecuteAsync(query, new { TweetId });
    }

    public async Task<List<TweetItem>> GetAll()
    {
        var query = $@"SELECT * FROM {TableNames.tweet} ORDER BY created_at DESC";

        using (var con = NewConnection)
            return (await con.QueryAsync<TweetItem>(query)).AsList();
    }

    public async Task<TweetItem> GetById(int TweetId)
    {
        var query = $@"SELECT * FROM {TableNames.tweet} WHERE tweet_id = @TweetId";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<TweetItem>(query, new { TweetId });
    }

    public async Task<List<TweetItem>> GetTweetsByUserId(int UserId)
    {
        var query = $@"SELECT * FROM {TableNames.tweet} WHERE user_id = @UserId";

        using (var con = NewConnection)
            return (await con.QueryAsync<TweetItem>(query, new { UserId })).AsList();
    }

    public async Task Update(TweetItem Item)
    {
        var query = $@"UPDATE {TableNames.tweet} SET title = @Title, 
        updated_at = @UpdatedAt WHERE Tweet_id = @TweetId";

        using (var con = NewConnection)
            await con.ExecuteAsync(query, Item);
    }
}