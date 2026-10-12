using System.Text.Json.Serialization;

namespace SupportManagement.Domain.Models;

public class Comment
{
    [JsonInclude]
    public Guid CommentId { get; private set; } = Guid.NewGuid();
    public string CommentContent { get; set; } = string.Empty;
    public DateTime CommentCreatedAtUtc { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public string CreatedAtLocal =>
    CommentCreatedAtUtc.ToLocalTime().ToString("yyyy-MM-dd HH:mm");
}
