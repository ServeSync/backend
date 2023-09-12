namespace ServeSync.Application.Common.Dtos;

public class PagingAndSortingRequestDto
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public virtual string Sorting { get; set; } = string.Empty;

    public PagingAndSortingRequestDto()
    {
        
    }

    public PagingAndSortingRequestDto(int page, int size, string sorting)
    {
        Page = page;
        Size = size;
        Sorting = sorting;
    }
}