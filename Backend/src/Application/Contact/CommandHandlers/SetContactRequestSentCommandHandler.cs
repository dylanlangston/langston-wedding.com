using Application.Contact.Commands;
using Domain.Contact.DomainEvents;
using Domain.Contact.Repositories;

namespace Application.Contact.CommandHandlers;

public class SetContactRequestSentCommandHandler : ICommandHandler<SetContactRequestSentCommand>
{
    private readonly IContactRequestRepository _contactRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SetContactRequestSentCommandHandler(IContactRequestRepository contactRepository, IUnitOfWork unitOfWork)
    {
        _contactRepository = contactRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(SetContactRequestSentCommand request, CancellationToken cancellationToken)
    {
        var contactRequest = await _contactRepository.GetByIdAsync(request.Id, cancellationToken);

        if (contactRequest == null) return Result.Failure($"Failed to find contact request with id: {request.Id}");

        contactRequest.RequestStatus = Domain.Contact.Aggregates.RequestStatus.Sent;

        contactRequest.RaiseDomainEvent(new ContactRequestEmailSentEvent(request.Id));

        _contactRepository.Update(contactRequest);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}