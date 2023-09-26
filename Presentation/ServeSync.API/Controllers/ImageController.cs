using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServeSync.Application.UseCases.Images;

namespace ServeSync.API.Controllers;

[Route("api/images")]
[ApiController]
public class ImageController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public ImageController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> UploadImageAsync([FromForm] IFormFile file)
    {
        var url = await _mediator.Send(new UploadImageCommand(file));
        return Ok(new
        {
            Url = url
        });
    }
}