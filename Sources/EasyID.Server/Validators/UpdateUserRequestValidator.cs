using FluentValidation;
using EasyID.Server.Models.Dto;
using System.Text.RegularExpressions;

namespace EasyID.Server.Validators
{
    public partial class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequestDto>
    {
        [GeneratedRegex(@"^[a-z][a-z0-9_-]*$", RegexOptions.Compiled)]
        private static partial Regex UsernameRegex();

        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
                .MaximumLength(64).WithMessage("Username cannot exceed 64 characters.")
                .Must(BeValidUsername).WithMessage("Username must start with a lowercase letter and contain only lowercase letters, digits, hyphens, and underscores.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MinimumLength(1).WithMessage("First name must contain at least one non-whitespace character.")
                .MaximumLength(100).WithMessage("First name cannot exceed 100 characters.")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("First name cannot be only whitespace.");

            RuleFor(x => x.LastName)
                .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters.")
                .Must(BeNullOrNotWhitespace).WithMessage("Last name cannot be only whitespace.");

            RuleFor(x => x.MiddleName)
                .MaximumLength(100).WithMessage("Middle name cannot exceed 100 characters.")
                .Must(BeNullOrNotWhitespace).WithMessage("Middle name cannot be only whitespace.");
        }

        private static bool BeValidUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return false;
            }
            return UsernameRegex().IsMatch(username);
        }

        private static bool BeNullOrNotWhitespace(string? value)
        {
            if (value == null)
            {
                return true;
            }
            return !string.IsNullOrWhiteSpace(value);
        }
    }
}
