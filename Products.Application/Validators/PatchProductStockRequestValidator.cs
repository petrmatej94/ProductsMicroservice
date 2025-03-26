using FluentValidation;
using Products.Contracts.Requests;

namespace Products.Application.Validators;

public class PatchProductStockRequestValidator : AbstractValidator<PatchProductStockRequest>
{
	public PatchProductStockRequestValidator()
	{
		RuleFor(x => x.QuantityInStock)
			.NotNull().WithMessage("QuantityInStock must be provided!")
			.GreaterThanOrEqualTo(0).WithMessage("QuantityInStock must be zero or greater.");
	}
}
