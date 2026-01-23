using ASP_NET_08._TaskFlow_DTOs.Models;

namespace ASP_NET_08._TaskFlow_DTOs.Services.Interfaces;

public interface ITaskItemService
{
    Task<IEnumerable<TaskItem>> GetAllAsync();
    Task<IEnumerable<TaskItem>> GetByProjectIdAsync(int projectId);
    Task<TaskItem?> GetByIdAsync(int id);
    Task<TaskItem> CreateAsync(TaskItem taskItem);
    Task<TaskItem?> UpdateAsync(int id, TaskItem taskItem);
    Task<bool> DeleteAsync(int id);
}
