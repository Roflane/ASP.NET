using ASP_NET_08._TaskFlow_DTOs.Enums;

namespace ASP_NET_08._TaskFlow_DTOs.DTOs.TaskItem_DTOs;

public class TaskItemResponseDto {
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ETaskStatus Status { get; set; } = ETaskStatus.ToDo;
    public int ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
}
