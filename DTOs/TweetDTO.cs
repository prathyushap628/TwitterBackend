using System.ComponentModel.DataAnnotations;

namespace Twitter.DTOs;

public record TweetCreateDTO
{
    [Required]
    [MinLength(3)]
    [MaxLength(90)]
    public string Title { get; set; }
}

public record TweetUpdateDTO
{
    [MinLength(3)]
    [MaxLength(255)]
    public string Title { get; set; } = null;

    public bool? Updated_at { get; set; } = null;
}