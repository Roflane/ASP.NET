using ASP_NET_08._TaskFlow_DTOs.DTOs.TaskItem_DTOs;
namespace ASP_NET_08._TaskFlow_DTOs.Services.Interfaces;

public interface ITaskItemService
{
    Task<IEnumerable<TaskItemResponseDto>> GetAllAsync();
    Task<IEnumerable<TaskItemResponseDto>> GetByProjectIdAsync(int projectId);
    Task<TaskItemResponseDto?> GetByIdAsync(int id);
    Task<TaskItemResponseDto> CreateAsync(CreateTaskItemDto dto);
    Task<TaskItemResponseDto?> UpdateAsync(int id, UpdateTaskItemDto dto);
    Task<bool> DeleteAsync(int id);
}
