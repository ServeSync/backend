using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Application.SeedWorks.Sessions;
using ServeSync.Domain.StudentManagement.ProofAggregate;
using ServeSync.Domain.StudentManagement.ProofAggregate.DomainServices;

namespace ServeSync.Application.UseCases.StudentManagement.Proofs.Commands;

public class CreateExternalProofCommandHandler : ICommandHandler<CreateExternalProofCommand, Guid>
{
    private readonly ICurrentUser _currentUser;
    private readonly IProofDomainService _proofDomainService;
    private readonly IProofRepository _proofRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateInternalProofCommandHandler> _logger;
    
    public CreateExternalProofCommandHandler(
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
    
    public async Task<Guid> Handle(CreateExternalProofCommand request, CancellationToken cancellationToken)
    {
        var proof = await _proofDomainService.CreateExternalProofAsync(
            request.Proof.Description,
            request.Proof.ImageUrl,
            request.Proof.AttendanceAt,
            request.Proof.RejectReason,
            Guid.Parse(_currentUser.ReferenceId),
            request.Proof.EventName,
            request.Proof.OrganizationName,
            request.Proof.Address,
            request.Proof.Role,
            request.Proof.StartAt,
            request.Proof.EndAt,
            request.Proof.Score,
            request.Proof.ActivityId);
        
        await _proofRepository.InsertAsync(proof);
        await _unitOfWork.CommitAsync();
        
        _logger.LogInformation("External proof {ProofId} created", proof.Id);
        return proof.Id;
    }
}