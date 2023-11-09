namespace ServeSync.Application.Common.Dtos;

public class PagingRequestDto
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    
    public PagingRequestDto()
    {
        
    }
    
    public PagingRequestDto(int page, int size)
    {
        Page = page < 1 ? 1 : page;
        Size = size < 1 ? 10 : size;
    }
}