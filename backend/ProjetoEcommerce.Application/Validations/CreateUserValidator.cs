using FluentValidation;
using ProjetoEcommerce.Application.Users.DTOs.Requests;

namespace ProjetoEcommerce.Application.Validations
{
    public class CreateUserValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório")
                .EmailAddress().WithMessage("Email inválido");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Senha é obrigatória")
                .MinimumLength(6).WithMessage("Senha deve ter no mínimo 6 caracteres")
                .Matches(@"[A-Z]").WithMessage("Senha deve conter letra maiúscula")
                .Matches(@"[0-9]").WithMessage("Senha deve conter número");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Primeiro nome é obrigatório")
                .MinimumLength(3).WithMessage("Primeiro nome deve ter no mínimo 3 caracteres");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Sobrenome é obrigatório")
                .MinimumLength(3).WithMessage("Sobrenome deve ter no mínimo 3 caracteres");

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\d{10,11}$").WithMessage("Telefone inválido");
        }
    }
}
