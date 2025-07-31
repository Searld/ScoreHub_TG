namespace ScoreHub_TG;

public class UserResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public Role Role { get; set; }
}

public enum Role
{
    Teacher,
    Student,
    Assistant
}