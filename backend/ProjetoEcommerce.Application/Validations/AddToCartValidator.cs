using FluentValidation;
using ProjetoEcommerce.Application.Cart.DTOs.Requests;

namespace ProjetoEcommerce.Application.Validations
{
    public class AddToCartValidator : AbstractValidator<AddToCartRequest>
    {
        public AddToCartValidator()
        {
            RuleFor(x => x.CartId)
                .NotEmpty().WithMessage("ID do carrinho é obrigatório");

            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("ID do produto é obrigatório");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantidade deve ser maior que zero")
                .LessThanOrEqualTo(1000).WithMessage("Quantidade máxima é 1000");
        }
    }
}
