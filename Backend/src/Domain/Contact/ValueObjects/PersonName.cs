namespace Domain.Contact.ValueObjects;

public class PersonName : ValueObject
{
    public string Value { get; }

    private PersonName() {}

    private PersonName(string value)
    {
        Value = value;
    }

    public static Result<PersonName> Create(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<PersonName>("Name cannot be empty.");
        }

        name = name.Trim();

        return new PersonName(name);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(PersonName email) => email.Value;
    public override string ToString() => Value;
}
