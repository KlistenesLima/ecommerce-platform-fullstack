using FluentValidation;
using ProjetoEcommerce.Application.Products.DTOs.Requests;

namespace ProjetoEcommerce.Application.Validations
{
    public class CreateProductValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .MinimumLength(5).WithMessage("Nome deve ter no mínimo 5 caracteres");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Descrição é obrigatória")
                .MinimumLength(10).WithMessage("Descrição deve ter no mínimo 10 caracteres");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Preço deve ser maior que zero");

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Quantidade deve ser maior ou igual a zero");

            RuleFor(x => x.Sku)
                .NotEmpty().WithMessage("SKU é obrigatório")
                .Matches(@"^[A-Z0-9-]+$").WithMessage("SKU deve conter apenas letras maiúsculas, números e hífen");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Categoria é obrigatória");
        }
    }
}
