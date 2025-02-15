namespace WebApi.Models.Users;

using System.ComponentModel.DataAnnotations;
using WebApi.Entities;

public class LoginUser
{
    [Required]
    public string Username { get; set; }

    public string Password { get; internal set; }
}