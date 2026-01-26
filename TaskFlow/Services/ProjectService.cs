using ASP_NET_08._TaskFlow_DTOs.Data;
using ASP_NET_08._TaskFlow_DTOs.DTOs;
using ASP_NET_08._TaskFlow_DTOs.Mappers;
using ASP_NET_08._TaskFlow_DTOs.Models;
using ASP_NET_08._TaskFlow_DTOs.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASP_NET_08._TaskFlow_DTOs.Services;

public class ProjectService(TaskFlowDbContext context) : IProjectService
{
    public async Task<ProjectResponseDto> CreateAsync(CreateProjectDto createDto) {
        var project = ProjectMapper.FromCreateDto(createDto);

        context.Projects.Add(project);
        await context.SaveChangesAsync();

        await context.Entry(project)
            .Collection(p => p.Tasks)
            .LoadAsync();

        return ProjectMapper.ToResponseDto(project);
    }

    public async Task<bool> DeleteAsync(int id) {
        var project = await context.Projects.FindAsync(id);
        if (project is null) return false;

        context.Projects.Remove(project);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<ProjectResponseDto>> GetAllAsync() {
        var projects = await context.Projects
            .Include(p => p.Tasks)
            .ToListAsync();

        return projects.Select(ProjectMapper.ToResponseDto);
    }

    public async Task<ProjectResponseDto?> GetByIdAsync(int id) {
        var project = await context.Projects
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == id);

        return project is null
            ? null
            : ProjectMapper.ToResponseDto(project);
    }

    public async Task<ProjectResponseDto?> UpdateAsync(int id, UpdateProjectDto updateDto) {
        var project = await context.Projects
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project is null) return null;

        ProjectMapper.UpdateFromDto(project, updateDto);
        await context.SaveChangesAsync();

        return ProjectMapper.ToResponseDto(project);
    }
}
