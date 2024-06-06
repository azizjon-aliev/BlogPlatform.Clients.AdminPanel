namespace BlogPlatform.Clients.AdminPanel.Contracts;

public class TagDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = String.Empty;

    public DateTime CreatedAt { get; set; }
}