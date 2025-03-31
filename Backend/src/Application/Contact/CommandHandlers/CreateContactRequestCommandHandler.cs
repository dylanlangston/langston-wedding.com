using Application.Contact.Commands;
using Domain.Contact.Aggregates;
using Domain.Contact.Repositories;
using Domain.Contact.ValueObjects;

namespace Application.Contact.CommandHandlers;

public class CreateContactRequestCommandHandler : ICommandHandler<CreateContactRequestCommand>
{
    private readonly IContactRequestRepository _contactRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateContactRequestCommandHandler(IContactRequestRepository contactRepository, IUnitOfWork unitOfWork)
    {
        _contactRepository = contactRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CreateContactRequestCommand request, CancellationToken cancellationToken)
    {
        var nameResult = PersonName.Create(request.Name);
        var emailResult = Email.Create(request.Email);

        var combinedResult = Result.Combine(nameResult, emailResult);
        if (!combinedResult.IsSuccess) return combinedResult;

        var contactResult = ContactRequest.Create(
            nameResult.Value,
            emailResult.Value,
            request.Message);

        if (!contactResult.IsSuccess) return contactResult;

        var contact = contactResult.Value;

        await _contactRepository.AddAsync(contact, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(contact);
    }
}