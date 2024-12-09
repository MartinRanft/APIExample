using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace ByteWizardApi.ApiAuth
{
    /// <summary>
    /// A custom Swagger operation filter that adds an API key header to operations requiring
    /// <see cref="ApiKeyAuthAttribute"/> for authentication.
    /// </summary>
    /// <remarks>
    /// This filter is applied to operations in the Swagger documentation to ensure that the necessary
    /// headers are documented for routes that require an API key for access. It checks for the presence
    /// of <see cref="ApiKeyAuthAttribute"/> on an action method or its declaring type, and if detected,
    /// a required API key header is added to the operation's parameters.
    /// </remarks>
    /// <remarks>
    /// This filter should be registered in the Swagger generation configuration to integrate the API key
    /// requirement into the Swagger UI. This helps API consumers understand which endpoints require an
    /// API key and what header to use when making requests.
    /// </remarks>
    public class AddApiKeyHeaderOperationFilter : IOperationFilter
    {
        
        /// <summary>
        /// Applies the API key header requirement to the OpenAPI operation if the method or declaring type
        /// contains the <see cref="ApiKeyAuthAttribute"/>.
        /// </summary>
        /// <param name="operation">The OpenAPI operation to which the API key header is added.</param>
        /// <param name="context">
        /// The context that provides information about the operation and related attributes for filtering.
        /// </param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            IEnumerable<object> authAttributes = context.MethodInfo.GetCustomAttributes(true)
                                                        .Union(context!.MethodInfo!.DeclaringType!.GetCustomAttributes(true));

            if(authAttributes.Any(a => a is ApiKeyAuthAttribute))
            {
                operation.Parameters ??= [];

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "ApiKey",
                    In = ParameterLocation.Header,
                    Required = true
                });
            }
        }
    }
}