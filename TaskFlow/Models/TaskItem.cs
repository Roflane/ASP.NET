using System.Text.Json.Serialization;
using ASP_NET_08._TaskFlow_DTOs.DTOs.TaskItem_DTOs;

namespace ASP_NET_08._TaskFlow_DTOs.Models;

public class TaskItem {
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskStatus Status { get; set; } = TaskStatus.ToDo;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }

    public int ProjectId { get; set; }

    [JsonIgnore]
    public virtual Project Project { get; set; } = null!;

    public TaskItem(CreateTaskItemDto dto) {
        Title = dto.Title;
        Description = dto.Description;
        ProjectId = dto.ProjectId;
        Status = TaskStatus.ToDo;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public TaskItem() { }

    public void UpdateFromDto(UpdateTaskItemDto dto) {
        Title = dto.Title;
        Description = dto.Description;

        if (Enum.TryParse<TaskStatus>(dto.Status, out var newStatus)) {
            Status = newStatus;
        }

        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public TaskItemResponseDto ToResponseDto() {
        return new TaskItemResponseDto {
            Id = Id,
            Title = Title,
            Description = Description,
            Status = Status.ToString(),
            ProjectId = ProjectId,
            ProjectName = Project?.Name ?? string.Empty
        };
    }
}

public enum TaskStatus {
    ToDo,
    InProgress,
    Done
}