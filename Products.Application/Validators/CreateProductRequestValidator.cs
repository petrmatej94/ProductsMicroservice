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

		RuleFor(x => x.Price)
			.GreaterThan(0).WithMessage("Price must be greater than 0!");

		RuleFor(x => x.Description)
			.MaximumLength(10000).WithMessage("Description must be at most 10000 characters!")
			.When(x => !string.IsNullOrWhiteSpace(x.Description));

		RuleFor(x => x.QuantityInStock)
			.GreaterThanOrEqualTo(0).WithMessage("Quantity must be zero or greater!");
	}
}
