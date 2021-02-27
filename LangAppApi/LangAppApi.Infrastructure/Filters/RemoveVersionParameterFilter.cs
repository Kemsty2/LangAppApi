using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using Microsoft.OpenApi.Any;

namespace LangAppApi.Infrastructure.Filters
{
    public class RemoveVersionParameterFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var versionParameter = operation.Parameters.FirstOrDefault(p => p.Name == "version");
            if(versionParameter!= null) operation.Parameters.Remove(versionParameter);

            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "culture",
                In = ParameterLocation.Query,
                Description = "Culture Query",
                Schema = new OpenApiSchema()
                {
                    Type = "string",
                    Enum = new List<IOpenApiAny>()
                    {
                        new OpenApiString("fr"),
                        new OpenApiString("en"),
                        new OpenApiString("nl"),
                    }
                }
            });
            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "ui-culture",
                In = ParameterLocation.Query,
                Description = "Culture Query",
                Schema = new OpenApiSchema()
                {
                    Type = "string",
                    Enum = new List<IOpenApiAny>()
                    {
                        new OpenApiString("fr"),
                        new OpenApiString("en"),
                        new OpenApiString("nl"),
                    }
                }
            });
        }
    }
}