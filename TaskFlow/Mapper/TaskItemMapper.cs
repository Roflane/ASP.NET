using ASP_NET_08._TaskFlow_DTOs.DTOs.TaskItem_DTOs;
using ASP_NET_08._TaskFlow_DTOs.Models;

namespace ASP_NET_08._TaskFlow_DTOs.Mappers;

public static class TaskItemMapper {
    public static TaskItem FromCreateDto(CreateTaskItemDto dto)
        => new()
        {
            Title = dto.Title,
            Description = dto.Description,
            ProjectId = dto.ProjectId,
            Status = Enums.ETaskStatus.ToDo,
            CreatedAt = DateTimeOffset.UtcNow
        };

    public static void UpdateFromDto(TaskItem task, UpdateTaskItemDto dto)
    {
        task.Title = dto.Title;
        task.Description = dto.Description;
        task.Status = dto.Status;
        task.UpdatedAt = DateTimeOffset.UtcNow;
    }

    public static TaskItemResponseDto ToResponseDto(TaskItem task)
        => new()
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            ProjectId = task.ProjectId,
            ProjectName = task.Project?.Name ?? string.Empty
        };
}