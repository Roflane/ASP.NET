using ASP_NET_08._TaskFlow_DTOs.DTOs;
using ASP_NET_08._TaskFlow_DTOs.Models;
using ASP_NET_08._TaskFlow_DTOs.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASP_NET_08._TaskFlow_DTOs.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectsController(IProjectService projectService) : ControllerBase {
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectResponseDto>>> GetAll() {
        var projects = await projectService.GetAllAsync();
        return Ok(projects);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectResponseDto>> GetById(int id)
    {
        var project = await projectService.GetByIdAsync(id);
        if (project is null) 
            return NotFound($"Project with ID {id} not found");
        return Ok(project);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectResponseDto>> Create([FromBody] CreateProjectDto createProjectDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var createdProject = await projectService.CreateAsync(createProjectDto);
        return CreatedAtAction(
            nameof(GetById),
            new { id = createdProject.Id }, 
            createdProject);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProjectResponseDto>> Update(int id, [FromBody] UpdateProjectDto updateProjectDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var updatedProject = await projectService.UpdateAsync(id, updateProjectDto);

        if(updatedProject is null) return NotFound($"Project with ID {id} not found");

        return Ok(updatedProject);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var isDeleted = await projectService.DeleteAsync(id);

        if (!isDeleted) return NotFound($"Project with ID {id} not found");

        return NoContent();
    }
}
