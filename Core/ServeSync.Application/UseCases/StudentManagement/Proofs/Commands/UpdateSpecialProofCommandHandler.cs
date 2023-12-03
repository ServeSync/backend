using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Application.SeedWorks.Sessions;
using ServeSync.Domain.StudentManagement.ProofAggregate;
using ServeSync.Domain.StudentManagement.ProofAggregate.DomainServices;
using ServeSync.Domain.StudentManagement.ProofAggregate.Exceptions;

namespace ServeSync.Application.UseCases.StudentManagement.Proofs.Commands;

public class UpdateSpecialProofCommandHandler : ICommandHandler<UpdateSpecialProofCommand>
{
    private readonly ICurrentUser _currentUser;
    private readonly IProofDomainService _proofDomainService;
    private readonly IProofRepository _proofRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateSpecialProofCommandHandler> _logger;
    
    public UpdateSpecialProofCommandHandler(
        ICurrentUser currentUser,
        IProofDomainService proofDomainService,
        IProofRepository proofRepository, 
        IUnitOfWork unitOfWork, 
        ILogger<UpdateSpecialProofCommandHandler> logger)
    {
        _currentUser = currentUser;
        _proofDomainService = proofDomainService;
        _proofRepository = proofRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task Handle(UpdateSpecialProofCommand request, CancellationToken cancellationToken)
    {
        var proof = await _proofRepository.FindByIdAsync(request.Id);
        if (proof == null)
        {
            throw new ProofNotFoundException(request.Id);
        }
        
        await _proofDomainService.UpdateSpecialProofAsync(
            proof,
            request.Proof.Description,
            request.Proof.ImageUrl,
            request.Proof.Title,
            request.Proof.Role,
            request.Proof.StartAt,
            request.Proof.EndAt,
            request.Proof.Score,
            request.Proof.ActivityId);
        
        _proofRepository.Update(proof);
        await _unitOfWork.CommitAsync();
        
        _logger.LogInformation("Special proof {ProofId} updated by {UserId}", proof.Id, _currentUser.Id);
    }
}