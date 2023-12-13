using System.ComponentModel.DataAnnotations;

namespace ServeSync.Application.UseCases.StudentManagement.Proofs.Dtos;

public class RejectProofDto
{
    [Required] 
    [MinLength(5)] 
    public string RejectReason { get; set; } = null!;
}