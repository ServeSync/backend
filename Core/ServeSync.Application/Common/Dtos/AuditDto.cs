namespace ServeSync.Application.Common.Dtos;

public class AuditDto
{
    public string? Id { get; set; }
    public DateTime At { get; set; }
    public string? FullName { get; set; }
    public string? ImageUrl { get; set; }
}