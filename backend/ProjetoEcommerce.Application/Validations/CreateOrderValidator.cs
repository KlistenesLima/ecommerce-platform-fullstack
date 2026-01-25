using FluentValidation;
using ProjetoEcommerce.Application.Orders.DTOs.Requests;

namespace ProjetoEcommerce.Application.Validations
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("ID do usuário é obrigatório");

            RuleFor(x => x.ShippingAddress)
                .NotEmpty().WithMessage("Endereço de entrega é obrigatório")
                .MinimumLength(10).WithMessage("Endereço deve ter no mínimo 10 caracteres");

            RuleFor(x => x.BillingAddress)
                .NotEmpty().WithMessage("Endereço de faturamento é obrigatório")
                .MinimumLength(10).WithMessage("Endereço deve ter no mínimo 10 caracteres");
        }
    }
}
