using System.ComponentModel.DataAnnotations;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TicketService.API.Validators
{
    /// <summary>
    /// Validates the sort order parameter by checking if it is one of the allowed values.
    /// </summary>
    public class SortOrderValidator : ValidationAttribute, IParameterFilter
    {
        /// <summary>
        /// Gets or sets the allowed values for the sort order.
        /// </summary>
        public string[] AllowedValues { get; set; } = new[] { "ASC", "DESC" };

        /// <summary>
        /// Initializes a new instance of the <see cref="SortOrderValidator"/> class.
        /// </summary>
        public SortOrderValidator() : base("Value must be one of the following: {0}.") { }

        /// <summary>
        /// Validates the sort order value.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the value is valid.</returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var strValue = value as string;
            if (!string.IsNullOrEmpty(strValue) && AllowedValues.Contains(strValue))
                return ValidationResult.Success;
            return new ValidationResult(FormatErrorMessage(string.Join(",", AllowedValues)));
        }

        /// <summary>
        /// Applies the sort order validation to the OpenAPI parameter.
        /// Look to /swagger/v1/swagger.json
        /// </summary>
        /// <param name="parameter">The OpenAPI parameter.</param>
        /// <param name="context">The parameter filter context.</param>
        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            var attributes = context.ParameterInfo?.GetCustomAttributes(true).OfType<SortOrderValidator>();
            if (attributes != null)
            {
                foreach (var attribute in attributes)
                {
                    parameter.Schema.Extensions.Add("pattern", new OpenApiString(string.Join("|", attribute.AllowedValues.Select(v => $"^{v}$"))));
                }
            }
        }
    }
}