using FluentValidation;
using Products.Contracts.Requests;

namespace Products.Application.Validators;

public class GetAllProductsRequestValidator : AbstractValidator<GetAllProductsRequest>
{
	public GetAllProductsRequestValidator()
	{
		RuleFor(x => x.Page)
			.GreaterThan(0).WithMessage("Page must be greater than 0!");

		RuleFor(x => x.PageSize)
			.GreaterThan(0).WithMessage("PageSize must be greater than 0!")
			.LessThanOrEqualTo(100).WithMessage("PageSize cannot exceed 100!");
	}
}
