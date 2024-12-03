
using API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BuggyController : BaseApiController
{
    [HttpGet("unauthorized")]
    public IActionResult GetUnauthorized()
    {
        return Unauthorized("You are not authorized to access this resource");
    }

    [HttpGet("badrequest")]
    public IActionResult GetBadRequest()
    {
        return BadRequest("This was a bad request");
    }

    [HttpGet("notfound")]
    public IActionResult GetNotFound()
    {
        return NotFound("Resource was not found");
    }

    [HttpGet("internalerror")]
    public IActionResult GetServerError()
    {
        throw new Exception("This is a server error");
    }

    [HttpPost("validationerror")]
    public IActionResult GetValidationError(CreateProductDto product)
    {
        return Ok();
    }
}
