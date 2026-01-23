namespace ASP_NET_08._TaskFlow_DTOs.DTOs.TaskItem_DTOs;

public class CreateTaskItemDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int ProjectId { get; set; }
}
