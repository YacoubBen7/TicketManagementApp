using System.ComponentModel.DataAnnotations;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
namespace TicketService.API.Validators;


/// <summary>
/// Represents a validator for sorting column names.
/// </summary>
/// <typeparam name="T">The type of entity.</typeparam>
public class SortColumnValidator<T> : ValidationAttribute, IParameterFilter
{
    /// <summary>
    /// Gets the type of the entity.
    /// </summary>
    public Type EntityType { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SortColumnValidator{T}"/> class.
    /// </summary>
    public SortColumnValidator() : base("Column name must be a valid property of the entity.")
    {
        EntityType = typeof(T);
    }

    /// <summary>
    /// Validates the specified value.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="validationContext">The validation context.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating whether the value is valid.</returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (EntityType != null)
        {
            var strValue = value as string;
            if (!string.IsNullOrEmpty(strValue) && EntityType.GetProperties().Any(p => p.Name == strValue))
                return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }

    /// <summary>
    /// Applies the validator to the OpenAPI parameter.
    /// </summary>
    /// <param name="parameter">The OpenAPI parameter.</param>
    /// <param name="context">The parameter filter context.</param>
    public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
    {
        var attributes = context.ParameterInfo?.GetCustomAttributes(true).OfType<SortColumnValidator<T>>();
        if (attributes != null)
        {
            foreach (var attribute in attributes)
            {
                var pattern = attribute.EntityType.GetProperties().Select(p => p.Name);
                parameter.Schema.Extensions.Add("pattern",new OpenApiString(string.Join("|",pattern.Select(v => $"^{v}$"))));
            }
        }
    }
}
