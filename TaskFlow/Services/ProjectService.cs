using ASP_NET_08._TaskFlow_DTOs.Data;
using ASP_NET_08._TaskFlow_DTOs.DTOs;
using ASP_NET_08._TaskFlow_DTOs.Models;
using ASP_NET_08._TaskFlow_DTOs.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASP_NET_08._TaskFlow_DTOs.Services;

public class ProjectService : IProjectService
{
    private readonly TaskFlowDbContext _context;

    public ProjectService(TaskFlowDbContext context)
    {
        _context = context;
    }

    public async Task<ProjectResponseDto> CreateAsync(CreateProjectDto createDto)
    {
        var project = new Project
        {
            Name = createDto.Name,
            Description = createDto.Description,
            CreatedAt = DateTime.UtcNow
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        await 
            _context.Entry(project)
            .Collection(p => p.Tasks)
            .LoadAsync();

       
        return MapToResponseDto(project);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var project = await _context.Projects.FindAsync(id);

        if (project is null) return false;

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<ProjectResponseDto>> GetAllAsync()
    {
        var projects = await _context.Projects
            .Include(p => p.Tasks)
            .ToListAsync();

       return projects.Select(p => MapToResponseDto(p));
    }

    public async Task<ProjectResponseDto?> GetByIdAsync(int id)
    {
        var project = await _context
              .Projects
              .Include(p => p.Tasks)
              .FirstOrDefaultAsync(p => p.Id == id);
        return MapToResponseDto(project!);
    }

    public async Task<ProjectResponseDto?> UpdateAsync(int id, UpdateProjectDto updateDto)
    {
        var project = await _context.Projects
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project is null) return null;

        project!.Name = updateDto.Name;
        project.Description = updateDto.Description;
        project.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToResponseDto(project);
    }

    private ProjectResponseDto MapToResponseDto(Project project)
    {
        return new ProjectResponseDto()
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            TaskCount = project.Tasks.Count
        };
    }
}
