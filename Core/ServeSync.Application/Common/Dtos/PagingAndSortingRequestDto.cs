namespace ServeSync.Application.Common.Dtos;

public class PagingAndSortingRequestDto : PagingRequestDto
{
    public virtual string? Sorting { get; set; } = string.Empty;

    public PagingAndSortingRequestDto()
    {
        
    }

    public PagingAndSortingRequestDto(int page, int size, string? sorting) : base(page, size)
    {
        Sorting = sorting;
    }
}