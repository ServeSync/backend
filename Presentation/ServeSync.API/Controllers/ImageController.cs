using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServeSync.Application.ImageUploader;
using ServeSync.Application.UseCases.Images;
using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.API.Controllers;

[Route("api/images")]
[ApiController]
public class ImageController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IImageUploader _imageUploader;
    
    public ImageController(IMediator mediator, IImageUploader imageUploader)
    {
        _mediator = mediator;
        _imageUploader = imageUploader;
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
    
    [HttpPost("assets")]
    public async Task<IActionResult> UploadAssetImageAsync([FromForm] IFormFile file)
    {
        var result = await _imageUploader.UploadAsync(file.FileName, file.OpenReadStream());
        if (!result.IsSuccess)
        {
            throw new ResourceInvalidOperationException(result.ErrorMessage!, "UploadImageFailed");
        }

        return Ok(new
        {
            Url = result.Url
        });
    }
}