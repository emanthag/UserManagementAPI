[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private static readonly List<User> Users = new();

    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            return Ok(Users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        try
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            return user is null
                ? NotFound($"User with ID {id} not found.")
                : Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }

    [HttpPost]
    public IActionResult Create([FromBody] User user)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            user.Id = Users.Count == 0 ? 1 : Users.Max(u => u.Id) + 1;
            Users.Add(user);

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] User updated)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user is null)
                return NotFound($"User with ID {id} not found.");

            user.FullName = updated.FullName;
            user.Email = updated.Email;
            user.Department = updated.Department;

            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        try
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user is null)
                return NotFound($"User with ID {id} not found.");

            Users.Remove(user);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }
}
