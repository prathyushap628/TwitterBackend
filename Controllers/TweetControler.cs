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
[Route("api/tweet")]
public class TweetController : ControllerBase
{
    private readonly ILogger<TweetController> _logger;
    private readonly ITweetRepository _tweet;

    private readonly ICommentRepository _comment;

    public TweetController(ILogger<TweetController> logger,
    ITweetRepository tweet, ICommentRepository comment)
    {
        _logger = logger;
        _tweet = tweet;
        _comment = comment;
    }

    private int GetUserIdFromClaims(IEnumerable<Claim> claims)
    {
        return Convert.ToInt32(claims.Where(x => x.Type == TwitterConstants.UserId).First().Value);
    }

    [HttpPost]
    public async Task<ActionResult<TweetItem>> CreateTweet([FromBody] TweetCreateDTO Data)
    {
        var userId = GetUserIdFromClaims(User.Claims);

        // Todo: get user tweets from db using userid

        // Todo: check if the tweets length is greater than or equals to 5 then send response as tweet limit exceeded
        //else create tweet

        List<TweetItem> usertweets = await _tweet.GetTweetsByUserId(userId);
        if (usertweets != null && usertweets.Count >= 5)
        {
            return BadRequest("Limit exceeded");
        }

        var toCreateItem = new TweetItem
        {
            Title = Data.Title.Trim(),
            UserId = userId,

        };

        // Insert into DB
        var createdItem = await _tweet.Create(toCreateItem);

        // Return the created Tweet
        return StatusCode(201, createdItem);
    }

    [HttpPut("{tweet_id}")]
    public async Task<ActionResult> UpdateTweet([FromRoute] int Tweet_id,
    [FromBody] TweetUpdateDTO Data)
    {
        var userId = GetUserIdFromClaims(User.Claims);

        var existingItem = await _tweet.GetById(Tweet_id);

        if (existingItem is null)
            return NotFound();

        if (existingItem.UserId != userId)
            return StatusCode(403, "You cannot update other's Tweet");

        var toUpdateItem = existingItem with
        {
            Title = Data.Title is null ? existingItem.Title : Data.Title.Trim(),
            // UpdatedAt = !Data.Updated_at.HasValue ? existingItem.UpdatedAt : Data.Updated_at.Value,
        };

        await _tweet.Update(toUpdateItem);

        return NoContent();
    }

    [HttpDelete("{tweet_id}")]
    public async Task<ActionResult> DeleteTweet([FromRoute] int tweet_id)
    {
        var userId = GetUserIdFromClaims(User.Claims);

        var existingItem = await _tweet.GetById(tweet_id);

        if (existingItem is null)
            return NotFound();

        if (existingItem.UserId != userId)
            return StatusCode(403, "You cannot delete other's Tweet");

        await _comment.DeleteByTweetId(tweet_id);

        await _tweet.Delete(tweet_id);

        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<List<TweetItem>>> GetAllTweets()
    {
        var allTweet = await _tweet.GetAll();
        return Ok(allTweet);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<TweetItem>> GetTweetById([FromRoute] int id)
    {
        var singleTweet = await _tweet.GetById(id);
        return Ok(singleTweet);
    }

}