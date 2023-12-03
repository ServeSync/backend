using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Application.SeedWorks.Sessions;
using ServeSync.Domain.StudentManagement.ProofAggregate;
using ServeSync.Domain.StudentManagement.ProofAggregate.DomainServices;

namespace ServeSync.Application.UseCases.StudentManagement.Proofs.Commands;

public class CreateInternalProofCommandHandler : ICommandHandler<CreateInternalProofCommand, Guid>
{
    private readonly ICurrentUser _currentUser;
    private readonly IProofDomainService _proofDomainService;
    private readonly IProofRepository _proofRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateInternalProofCommandHandler> _logger;
    
    public CreateInternalProofCommandHandler(
        ICurrentUser currentUser,
        IProofDomainService proofDomainService,
        IProofRepository proofRepository, 
        IUnitOfWork unitOfWork, 
        ILogger<CreateInternalProofCommandHandler> logger)
    {
        _currentUser = currentUser;
        _proofDomainService = proofDomainService;
        _proofRepository = proofRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task<Guid> Handle(CreateInternalProofCommand request, CancellationToken cancellationToken)
    {
        var proof = await _proofDomainService.CreateInternalProofAsync(
            request.Proof.Description,
            request.Proof.ImageUrl,
            request.Proof.AttendanceAt,
            Guid.Parse(_currentUser.ReferenceId),
            request.Proof.EventId,
            request.Proof.EventRoleId,
            DateTime.UtcNow);
        
        await _proofRepository.InsertAsync(proof);
        await _unitOfWork.CommitAsync();
        
        _logger.LogInformation("Proof {ProofId} created", proof.Id);
        return proof.Id;
    }
}