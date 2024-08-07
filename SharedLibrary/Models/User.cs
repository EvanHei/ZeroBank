namespace SharedLibrary.Models;

public class User
{
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public int Id { get; set; }

    public User(string username, string passwordHash)
    {
        Username = username;
        PasswordHash = passwordHash;
    }
}