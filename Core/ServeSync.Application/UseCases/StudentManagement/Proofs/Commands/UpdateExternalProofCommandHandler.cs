using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Application.SeedWorks.Sessions;
using ServeSync.Domain.StudentManagement.ProofAggregate;
using ServeSync.Domain.StudentManagement.ProofAggregate.DomainServices;
using ServeSync.Domain.StudentManagement.ProofAggregate.Exceptions;

namespace ServeSync.Application.UseCases.StudentManagement.Proofs.Commands;

public class UpdateExternalProofCommandHandler : ICommandHandler<UpdateExternalProofCommand>
{
    private readonly ICurrentUser _currentUser;
    private readonly IProofDomainService _proofDomainService;
    private readonly IProofRepository _proofRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateExternalProofCommandHandler> _logger;
    
    public UpdateExternalProofCommandHandler(
        ICurrentUser currentUser,
        IProofDomainService proofDomainService,
        IProofRepository proofRepository, 
        IUnitOfWork unitOfWork, 
        ILogger<UpdateExternalProofCommandHandler> logger)
    {
        _currentUser = currentUser;
        _proofDomainService = proofDomainService;
        _proofRepository = proofRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task Handle(UpdateExternalProofCommand request, CancellationToken cancellationToken)
    {
        var proof = await _proofRepository.FindByIdAsync(request.Id);
        if (proof == null)
        {
            throw new ProofNotFoundException(request.Id);
        }
        
        await _proofDomainService.UpdateExternalProofAsync(
            proof,
            request.Proof.Description,
            request.Proof.ImageUrl,
            request.Proof.AttendanceAt,
            request.Proof.EventName,
            request.Proof.OrganizationName,
            request.Proof.Address,
            request.Proof.Role,
            request.Proof.StartAt,
            request.Proof.EndAt,
            request.Proof.Score,
            request.Proof.ActivityId);
        
        _proofRepository.Update(proof);
        await _unitOfWork.CommitAsync();
        
        _logger.LogInformation("External proof {ProofId} updated by {UserId}", proof.Id, _currentUser.Id);
    }
}