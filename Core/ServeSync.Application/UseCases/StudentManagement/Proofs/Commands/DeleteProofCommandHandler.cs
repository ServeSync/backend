using Microsoft.Extensions.Logging;
using ServeSync.Application.SeedWorks.Cqrs;
using ServeSync.Application.SeedWorks.Data;
using ServeSync.Domain.StudentManagement.ProofAggregate;
using ServeSync.Domain.StudentManagement.ProofAggregate.DomainServices;
using ServeSync.Domain.StudentManagement.ProofAggregate.Exceptions;

namespace ServeSync.Application.UseCases.StudentManagement.Proofs.Commands;

public class DeleteProofCommandHandler : ICommandHandler<DeleteProofCommand>
{
    private readonly IProofRepository _proofRepository;
    private readonly IProofDomainService _proofDomainService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteProofCommandHandler> _logger;

    public DeleteProofCommandHandler(
        IProofRepository proofRepository,
        IProofDomainService proofDomainService,
        IUnitOfWork unitOfWork,
        ILogger<DeleteProofCommandHandler> logger)
    {
        _proofRepository = proofRepository;
        _proofDomainService = proofDomainService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task Handle(DeleteProofCommand request, CancellationToken cancellationToken)
    {
        var proof = await _proofRepository.FindByIdAsync(request.Id);
        if (proof == null)
        {
            throw new ProofNotFoundException(request.Id);
        }
        
        _proofDomainService.Delete(proof);
        await _unitOfWork.CommitAsync();
        
        _logger.LogInformation("Proof with id {ProofId} was deleted!", request.Id);
    }
}
