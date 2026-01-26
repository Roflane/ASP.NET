using System.Text.Json.Serialization;
using ASP_NET_08._TaskFlow_DTOs.DTOs.TaskItem_DTOs;
using ASP_NET_08._TaskFlow_DTOs.Enums;

namespace ASP_NET_08._TaskFlow_DTOs.Models;

public class TaskItem {
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ETaskStatus Status { get; set; } = ETaskStatus.ToDo;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public int ProjectId { get; set; }

    [JsonIgnore]
    public virtual Project? Project { get; set; }
}
