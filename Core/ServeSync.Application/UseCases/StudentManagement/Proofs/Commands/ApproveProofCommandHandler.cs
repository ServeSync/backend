using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Domain.StudentManagement.ProofAggregate;
using ServeSync.Domain.StudentManagement.ProofAggregate.DomainServices;
using ServeSync.Domain.StudentManagement.ProofAggregate.Exceptions;

namespace ServeSync.Application.UseCases.StudentManagement.Proofs.Commands;

public class ApproveProofCommandHandler : ICommandHandler<ApproveProofCommand>
{
    private readonly IProofRepository _proofRepository;
    private readonly IProofDomainService _proofDomainService;
    private readonly ILogger<ApproveProofCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    
    public ApproveProofCommandHandler(
        IProofRepository proofRepository, 
        IProofDomainService proofDomainService,
        IUnitOfWork unitOfWork,
        ILogger<ApproveProofCommandHandler> logger)
    {
        _proofRepository = proofRepository;
        _proofDomainService = proofDomainService;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(ApproveProofCommand request, CancellationToken cancellationToken)
    {
        var proof = await _proofRepository.FindByIdAsync(request.Id);
        if (proof == null)
        {
            throw new ProofNotFoundException(request.Id);
        }

        _proofDomainService.ApproveProof(proof);
        _proofRepository.Update(proof);
        await _unitOfWork.CommitAsync();
        
        _logger.LogInformation("Proof {Id} approved successfully!", request.Id);
    }
}