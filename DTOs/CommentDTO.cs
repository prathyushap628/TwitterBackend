using System.ComponentModel.DataAnnotations;

namespace Twitter.DTOs;

public record CommentCreateDTO
{
    [Required]
    [MinLength(3)]
    [MaxLength(255)]
    public string Text { get; set; }

    [Required]
    public int TweetId { get; set; }
}

public record CommentUpdateDTO
{
    [MinLength(3)]
    [MaxLength(255)]
    public string Text { get; set; } = null;

    public bool? Updated_at { get; set; } = null;
}