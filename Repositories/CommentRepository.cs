using Twitter.Models;
using Dapper;
using Twitter.Utilities;
using Twitter.Repositories;
using Tweet.Models;
using Twitter.Models;

namespace Twitter.Repositories;

public interface ICommentRepository
{
    Task<Comment> Create(Comment Item);

    Task Delete(int CommentId);
    Task<List<Comment>> GetAll();
    Task<Comment> GetById(int CommentId);
    Task<List<Comment>> GetCommentsByTweetId(int TweetId);
    Task DeleteByTweetId(int TweetId);
}

public class CommentRepository : BaseRepository, ICommentRepository
{
    public CommentRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<Comment> Create(Comment Item)
    {
        var query = $@"INSERT INTO {TableNames.comment} (text, user_id, tweet_id) 
        VALUES (@Text, @UserId ,@TweetId) RETURNING *";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Comment>(query, Item);
    }

    public async Task Delete(int CommentId)
    {
        var query = $@"DELETE FROM {TableNames.comment} WHERE comment_id = @CommentId";

        using (var con = NewConnection)
            await con.ExecuteAsync(query, new { CommentId });
    }

    public async Task DeleteByTweetId(int TweetId)
    {
        var query = $@"DELETE FROM {TableNames.comment} WHERE tweet_id = @TweetId";

        using (var con = NewConnection)
            await con.ExecuteAsync(query, new { TweetId });
    }


public async Task<List<Comment>> GetAll()
{
    var query = $@"SELECT * FROM {TableNames.comment} ORDER BY created_at DESC";

    using (var con = NewConnection)
        return (await con.QueryAsync<Comment>(query)).AsList();
}



public async Task<Comment> GetById(int CommentId)
{
    var query = $@"SELECT * FROM {TableNames.comment} WHERE comment_id = @CommentId";

    using (var con = NewConnection)
        return await con.QuerySingleOrDefaultAsync<Comment>(query, new { CommentId });
}
public async Task<List<Comment>> GetCommentsByTweetId(int TweetId)
{
    var query = $@"SELECT * FROM {TableNames.comment} WHERE tweet_id = @TweetId";

    using (var con = NewConnection)
        return (await con.QueryAsync<Comment>(query, new { TweetId })).AsList();
}

    // public async Task Update(Comment Item)
    // {
    //     var query = $@"UPDATE {TableNames.comment} SET title = @Title, 
    //     updated_at = @UpdatedAt WHERE Tweet_id = @TweetId";

    //     using (var con = NewConnection)
    //         await con.ExecuteAsync(query, Item);
    // }
}
