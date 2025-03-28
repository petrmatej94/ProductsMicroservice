using FluentValidation;
using Products.Contracts.Requests;

namespace Products.Application.Validators;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
	public CreateProductRequestValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("Product name is required!")
			.MaximumLength(100).WithMessage("Product name must be at most 100 characters.");

		RuleFor(x => x.ImageUrl)
			.NotEmpty().WithMessage("Image URL is required!")
			.Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
			.WithMessage("Image URL must be a valid absolute URL.");
	}
}
