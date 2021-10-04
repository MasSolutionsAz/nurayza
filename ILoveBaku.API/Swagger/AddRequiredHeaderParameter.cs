using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace ILoveBaku.API.Swagger
{
    public class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "culture",
                Schema = new OpenApiSchema { Type = "string"},
                Required = true,
                Description = "Header-da culture gondermeden api-dan cavab ala bilmeyeceksiniz.",
                AllowEmptyValue = false,
                Style = ParameterStyle.Simple,
                In = ParameterLocation.Header
            });
        }
    }
}
