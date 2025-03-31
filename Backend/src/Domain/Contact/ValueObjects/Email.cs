using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace Domain.Contact.ValueObjects;

public class Email : ValueObject
{
    [EmailAddress]
    public string Value { get; }

    private Email() {}

    private Email(string value)
    {
        Value = value;
    }

    private static readonly Regex _emailRegex = new Regex(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.Singleline,
        TimeSpan.FromMilliseconds(100));

    public static Result<Email> Create(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return Result.Failure<Email>("Email cannot be empty.");
        }

        email = email.Trim();

        if (!_emailRegex.IsMatch(email))
        {
            return Result.Failure<Email>("Email format is invalid.");
        }

        return new Email(email);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(Email email) => email.Value;
    public override string ToString() => Value;
}
