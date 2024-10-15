using System.ComponentModel.DataAnnotations;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TicketService.API.Validators;

public class StatusValidator<T> : ValidationAttribute, IParameterFilter
{
    public StatusValidator() : base("The Role must be one of these [{0}]")
    { }

    public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
    {
        var attributes = context.ParameterInfo?.GetCustomAttributes(true).OfType<StatusValidator<T>>();
        if (attributes != null)
        {
            foreach (var attribute in attributes)
            {
                parameter.Schema.Extensions.Add("pattern", new OpenApiString(string.Join("|", Enum.GetNames(typeof(T)).Select(v => $"^{v}$"))));
            }
        }
    }


    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        Type type = typeof(T);
        if (value is string role)
        {
            if (Enum.TryParse(type, role, true, out object? _))
            {
                return ValidationResult.Success;
            }
        }
        return new ValidationResult(FormatErrorMessage(string.Join(",", Enum.GetNames(type))));
    }



}
