using ASP_NET_08._TaskFlow_DTOs.Data;
using ASP_NET_08._TaskFlow_DTOs.DTOs.TaskItem_DTOs;
using ASP_NET_08._TaskFlow_DTOs.Mappers;
using ASP_NET_08._TaskFlow_DTOs.Models;
using ASP_NET_08._TaskFlow_DTOs.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASP_NET_08._TaskFlow_DTOs.Services;

public class TaskItemService(TaskFlowDbContext context) : ITaskItemService {
    public async Task<TaskItemResponseDto> CreateAsync(CreateTaskItemDto dto)
    {
        var projectExists = await context.Projects.AnyAsync(p => p.Id == dto.ProjectId);
        if (!projectExists) throw new ArgumentException($"Project with ID {dto.ProjectId} not found");

        var task = TaskItemMapper.FromCreateDto(dto);   

        context.TaskItems.Add(task);
        await context.SaveChangesAsync();

        await context.Entry(task).Reference(t => t.Project).LoadAsync();

        return TaskItemMapper.ToResponseDto(task);
    }

    public async Task<TaskItemResponseDto?> UpdateAsync(int id, UpdateTaskItemDto dto)
    {
        var task = await context.TaskItems.Include(t => t.Project).FirstOrDefaultAsync(t => t.Id == id);
        if (task is null) return null;

        TaskItemMapper.UpdateFromDto(task, dto);
        await context.SaveChangesAsync();

        return TaskItemMapper.ToResponseDto(task);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var task = await context.TaskItems.FindAsync(id);
        if (task is null) return false;

        context.TaskItems.Remove(task);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<TaskItemResponseDto?> GetByIdAsync(int id)
    {
        var task = await context.TaskItems.Include(t => t.Project).FirstOrDefaultAsync(t => t.Id == id);
        return task is null ? null : TaskItemMapper.ToResponseDto(task);
    }

    public async Task<IEnumerable<TaskItemResponseDto>> GetAllAsync()
    {
        var tasks = await context.TaskItems.Include(t => t.Project).ToListAsync();
        return tasks.Select(TaskItemMapper.ToResponseDto);
    }

    public async Task<IEnumerable<TaskItemResponseDto>> GetByProjectIdAsync(int projectId)
    {
        var tasks = await context.TaskItems
            .Include(t => t.Project)
            .Where(t => t.ProjectId == projectId)
            .ToListAsync();

        return tasks.Select(TaskItemMapper.ToResponseDto);
    }
}
