using ASP_NET_08._TaskFlow_DTOs.DTOs;
using ASP_NET_08._TaskFlow_DTOs.Models;

namespace ASP_NET_08._TaskFlow_DTOs.Mappers;

public static class ProjectMapper {
    public static Project FromCreateDto(CreateProjectDto dto)
        => new() {
            Name = dto.Name,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow
        };

    public static void UpdateFromDto(Project project, UpdateProjectDto dto) {
        project.Name = dto.Name;
        project.Description = dto.Description;
        project.UpdatedAt = DateTime.UtcNow;
    }

    public static ProjectResponseDto ToResponseDto(Project project)
        => new() {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            TaskCount = project.Tasks?.Count ?? 0
        };
}