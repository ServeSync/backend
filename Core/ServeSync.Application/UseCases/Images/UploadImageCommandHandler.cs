using ServeSync.Application.ImageUploader;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Domain.SeedWorks.Exceptions.Resources;

namespace ServeSync.Application.UseCases.Images;

public class UploadImageCommandHandler : ICommandHandler<UploadImageCommand, string>
{
    private readonly IImageUploader _imageUploader;
    
    public UploadImageCommandHandler(IImageUploader imageUploader)
    {
        _imageUploader = imageUploader;
    }
    
    public async Task<string> Handle(UploadImageCommand request, CancellationToken cancellationToken)
    {
        var result = await _imageUploader.UploadAsync(request.File.FileName, request.File.OpenReadStream());
        if (!result.IsSuccess)
        {
            throw new ResourceInvalidOperationException(result.ErrorMessage!, "UploadImageFailed");
        }

        return result.Url!;
    }
}