using System.Text.Json.Serialization;

namespace Twitter.Models;

public record TweetItem
{
    [JsonPropertyName("tweet_id")]
    public int TweetId { get; set; }

    [JsonPropertyName("user_id")]
    public int UserId { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;




}