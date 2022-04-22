using Twitter.Models;
using Twitter.Repositories;
using Microsoft.AspNetCore.Mvc;
using Twitter.DTOs;
using Microsoft.AspNetCore.Authorization;
using Twitter.Utilities;
using System.Security.Claims;
using Tweet.Models;

namespace Twitter.Controllers;

[ApiController]
[Authorize]
[Route("api/comment")]
public class CommentController : ControllerBase
{
    private readonly ILogger<CommentController> _logger;
    private readonly ICommentRepository _comment;

    public CommentController(ILogger<CommentController> logger,
    ICommentRepository comment)
    {
        _logger = logger;
        _comment = comment;
    }

    private int GetUserIdFromClaims(IEnumerable<Claim> claims)
    {
        return Convert.ToInt32(claims.Where(x => x.Type == TwitterConstants.UserId).First().Value);
    }

    [HttpPost]
    public async Task<ActionResult<Comment>> CreateComment([FromBody] CommentCreateDTO Data)
    {
        var userId = GetUserIdFromClaims(User.Claims);

        var toCreateItem = new Comment
        {
            Text = Data.Text.Trim(),
            UserId = userId,
            TweetId = Data.TweetId

        };

        // Insert into DB
        var createdItem = await _comment.Create(toCreateItem);

        // Return the created Tweet
        return StatusCode(201, createdItem);
    }

    // [HttpPut("{comment_id}")]
    // public async Task<ActionResult> UpdateTweet([FromRoute] int Tweet_id,
    // [FromBody] TweetUpdateDTO Data)
    // {
    //     var userId = GetUserIdFromClaims(User.Claims);

    //     var existingItem = await _tweet.GetById(Tweet_id);

    //     if (existingItem is null)
    //         return NotFound();

    //     if (existingItem.UserId != userId)
    //         return StatusCode(403, "You cannot update other's Tweet");

    //     var toUpdateItem = existingItem with
    //     {
    //         Title = Data.Title is null ? existingItem.Title : Data.Title.Trim(),
    //         // UpdatedAt = !Data.Updated_at.HasValue ? existingItem.UpdatedAt : Data.Updated_at.Value,
    //     };

    //     await _tweet.Update(toUpdateItem);

    //     return NoContent();
    // }

    [HttpDelete("{comment_id}")]
    public async Task<ActionResult> DeleteComment([FromRoute] int comment_id)
    {
        var userId = GetUserIdFromClaims(User.Claims);

        var existingItem = await _comment.GetById(comment_id);

        if (existingItem is null)
            return NotFound();

        if (existingItem.UserId != userId)
            return StatusCode(403, "You cannot delete other's Comment");

        await _comment.Delete(comment_id);

        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<List<Comment>>> GetAllComments()
    {
        var allComment = await _comment.GetAll();
        return Ok(allComment);
    }

    [HttpGet("{tweet_id}")]
    public async Task<ActionResult<List<Comment>>> GetAllCommentsByTweetId([FromRoute] int tweet_id)
    {
        var allComment = await _comment.GetCommentsByTweetId(tweet_id);
        return Ok(allComment);
    }
}