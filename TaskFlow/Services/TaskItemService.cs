using ASP_NET_08._TaskFlow_DTOs.Data;
using ASP_NET_08._TaskFlow_DTOs.Models;
using ASP_NET_08._TaskFlow_DTOs.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASP_NET_08._TaskFlow_DTOs.Services;

public class TaskItemService(TaskFlowDbContext context) : ITaskItemService {
    public async Task<TaskItem> CreateAsync(TaskItem taskItem)
    {
        var projectExixts = await context
                                        .Projects
                                        .AnyAsync(p => p.Id == taskItem.ProjectId);

        if (!projectExixts)
            throw new 
                ArgumentException($"Project with ID {taskItem.ProjectId} not found");

        taskItem.CreatedAt = DateTime.UtcNow;
        taskItem.UpdatedAt = null;
        taskItem.Status = Models.TaskStatus.ToDo;

        context.TaskItems.Add(taskItem);
        await context.SaveChangesAsync();

        await context
            .Entry(taskItem)
            .Reference(t => t.Project)
            .LoadAsync();

        return taskItem;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var task = await context.TaskItems.FindAsync(id);
        if (task is null) return false;

        context.TaskItems.Remove(task);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        return await context
                        .TaskItems
                        .Include(t => t.Project)
                        .ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await context
                         .TaskItems
                         .Include(t => t.Project)
                         .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<TaskItem>> GetByProjectIdAsync(int projectId)
    {
        return await context
                            .TaskItems
                            .Include(t => t.Project)
                            .Where(t => t.ProjectId == projectId)
                            .ToListAsync();
    }

    public async Task<TaskItem?> UpdateAsync(int id, TaskItem taskItem)
    {
        var task = await context
                            .TaskItems
                            .Include(t => t.Project)
                            .FirstOrDefaultAsync(t => t.Id == id);

        if (task is null) return null;

        task.Title = taskItem.Title;
        task.Description = taskItem.Description;
        task.Status = taskItem.Status;
        task.UpdatedAt = DateTime.Now;

        await context.SaveChangesAsync();

        return task;
    }
}
