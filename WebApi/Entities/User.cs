namespace WebApi.Entities;

using System.Text.Json.Serialization;
using WebApi.Models.Users;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Username { get; internal set; }
    public string PasswordHash { get; internal set; }
    public UserRole Role { get; set; }
}