﻿using System.Text.RegularExpressions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using OpenApiOpType = Microsoft.OpenApi.Models.OperationType;

namespace Smartstore.Web.Api.Swagger
{
    /// <summary>
    /// Removes Open API paths and schemas that should not be displayed in the Swagger documentation.
    /// </summary>
    /// <remarks>
    /// It would be nice if we could strip off the OData route here: /odata/v1/categories({key}) -> /categories({key})
    /// but that would let Swagger execute against /categories({key}) always resulting in 404 NotFound.
    /// To achieve this a custom Swagger template would be required. Perhaps there is an extension somewhere.
    /// </remarks>
    internal class SwaggerDocumentFilter : IDocumentFilter
    {
        private static readonly Regex _pathsToIgnore = new(@"[a-z0-9\/](\$count|\{key\})(/|\z)", 
            RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex _schemasToIgnore = new(@"(Microsoft\.AspNetCore\.OData\.|System\.Collections\.Generic\.KeyValuePair).+",
            RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            FilterSchemas(context);
            FilterPaths(swaggerDoc, context);
        }

        // Removes unexpected, unwanted schemas.
        private static void FilterSchemas(DocumentFilterContext context)
        {
            foreach (var item in context.SchemaRepository.Schemas)
            {
                if (_schemasToIgnore.IsMatch(item.Key))
                {
                    context.SchemaRepository.Schemas.Remove(item.Key);
                }
            }
        }

        // Removes duplicate and unusual, unexpected documents.
        private static void FilterPaths(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var apiDescription = context?.ApiDescriptions?.FirstOrDefault();
            var routeTemplate = apiDescription?.ActionDescriptor?.AttributeRouteInfo?.Template?.EnsureStartsWith('/');

            foreach (var item in swaggerDoc.Paths)
            {
                //$"{item.Key} {string.Join(",", item.Value.Operations.Select(x => x.Key))}".Dump();

                var path = item.Key;
                var removePath = path.EqualsNoCase(routeTemplate)
                    || (item.Value.Operations.Any(x => x.Key == OpenApiOpType.Get) && _pathsToIgnore.IsMatch(path));

                if (removePath)
                {
                    swaggerDoc.Paths.Remove(path);
                }
            }
        }
    }
}
