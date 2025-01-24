using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace mapa_back.Helpers
{
    public class AuthorizeOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasAuthorize = (context.MethodInfo.DeclaringType?.GetCustomAttributes(true)
                                        .OfType<AuthorizeAttribute>()
                                        .Any() ?? false) ||
                                    context.MethodInfo
                                        .GetCustomAttributes(true)
                                        .OfType<AuthorizeAttribute>()
                                        .Any();
            if (hasAuthorize)
            {
                // Usuwanie wymagań dotyczących autoryzacji dla endpointów z `[AllowAnonymous]`
                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                // Definition name. 
                                // Should exactly match the one given in the service configuration
                                Id = "Bearer"
                            }
                        }, new string[0]
                    }
                });
            }
        }
    }
}
