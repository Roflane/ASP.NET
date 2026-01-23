using ASP_NET_08._TaskFlow_DTOs.DTOs;
using ASP_NET_08._TaskFlow_DTOs.Models;

namespace ASP_NET_08._TaskFlow_DTOs.Services.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectResponseDto>> GetAllAsync();
    Task<ProjectResponseDto?> GetByIdAsync(int id);
    Task<ProjectResponseDto> CreateAsync(CreateProjectDto createDto);
    Task<ProjectResponseDto?> UpdateAsync(int id, UpdateProjectDto updateDto);
    Task<bool> DeleteAsync(int id);

}
