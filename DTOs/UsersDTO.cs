using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Twitter.DTOs;

public record UserLoginDTO
{


    [Required]
    [JsonPropertyName("email")]
    [MinLength(3)]
    [MaxLength(255)]
    public string Email { get; set; }

    [Required]
    [JsonPropertyName("password")]
    [MaxLength(255)]
    public string Password { get; set; }
}

public record UserLoginResDTO
{
    [JsonPropertyName("token")]
    public string Token { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("user_id")]
    public int UserId { get; set; }
    [JsonPropertyName("user_name")]
    public string UserName { get; set; }
}

public record UserCreateDTO
{


    [Required]
    [JsonPropertyName("email")]
    [MinLength(3)]
    [MaxLength(255)]
    public string Email { get; set; }
    [Required]
    [JsonPropertyName("name")]
    [MinLength(3)]
    [MaxLength(255)]
    public string UserName { get; set; }

    [Required]
    [JsonPropertyName("password")]
    [MaxLength(255)]
    public string Password { get; set; }
}


public record UserCreateResDTO
{


    [Required]
    [JsonPropertyName("email")]
    [MinLength(3)]
    [MaxLength(255)]
    public string Email { get; set; }
    [Required]
    [JsonPropertyName("name")]
    [MinLength(3)]
    [MaxLength(255)]
    public string UserName { get; set; }

    [Required]
    [JsonPropertyName("user_id")]
    [MaxLength(255)]
    public string UserId { get; set; }
}

public record UserLoginUpdateDTO
{



    [Required]
    [JsonPropertyName("name")]
    [MinLength(3)]
    [MaxLength(255)]
    public string UserName { get; set; }


}
