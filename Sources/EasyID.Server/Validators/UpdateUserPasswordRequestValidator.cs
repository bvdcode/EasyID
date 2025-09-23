using FluentValidation;
using EasyID.Server.Models.Dto;

namespace EasyID.Server.Validators
{
    public class UpdateUserPasswordRequestValidator : AbstractValidator<UpdateUserPasswordRequestDto>
    {
        public UpdateUserPasswordRequestValidator()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty().WithMessage("Old password is required.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(8)
                .WithMessage("New password must be at least 8 characters long.")
                .MaximumLength(1024)
                .WithMessage("New password must not exceed 128 characters.");
        }
    }
}
