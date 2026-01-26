using ASP_NET_08._TaskFlow_DTOs.DTOs.TaskItem_DTOs;
using ASP_NET_08._TaskFlow_DTOs.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASP_NET_08._TaskFlow_DTOs.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TaskItemsController(ITaskItemService taskItemService) : ControllerBase {
    // GET: api/TaskItems
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItemResponseDto>>> GetAll()
    {
        var tasks = await taskItemService.GetAllAsync();
        return Ok(tasks);
    }

    // GET: api/TaskItems/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItemResponseDto>> GetById(int id)
    {
        var task = await taskItemService.GetByIdAsync(id);
        if (task is null) return NotFound($"Task with ID {id} not found");
        return Ok(task);
    }

    // GET: api/TaskItems/project/3
    [HttpGet("project/{projectId}")]
    public async Task<ActionResult<IEnumerable<TaskItemResponseDto>>> GetByProjectId(int projectId)
    {
        var tasks = await taskItemService.GetByProjectIdAsync(projectId);
        return Ok(tasks);
    }

    // POST: api/TaskItems
    [HttpPost]
    public async Task<ActionResult<TaskItemResponseDto>> Create([FromBody] CreateTaskItemDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var task = await taskItemService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // PUT: api/TaskItems/5
    [HttpPut("{id}")]
    public async Task<ActionResult<TaskItemResponseDto>> Update(int id, [FromBody] UpdateTaskItemDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var task = await taskItemService.UpdateAsync(id, dto);
        if (task is null) return NotFound($"Task with ID {id} not found");

        return Ok(task);
    }

    // DELETE: api/TaskItems/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var isDeleted = await taskItemService.DeleteAsync(id);
        if (!isDeleted) return NotFound($"Task with ID {id} not found");

        return NoContent();
    }
}
