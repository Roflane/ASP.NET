using System.ComponentModel;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ASP_NET_08._TaskFlow_DTOs;

public class SwaggerSchemeFilter {
    public void Apply(OpenApiSchema schema, SchemaFilterContext context) {
        // if (context.Type is null) return;
        // var props = context.Type.GetProperties();
        // foreach (var prop in props) {
        //     var defaultValueAttribute = prop.GetCustomAttributes<DefaultValueAttribute>();
        //     if (defaultValueAttribute.Any()) {
        //         var propName = $"{char.ToLowerInvariant(prop.Name[0])}{prop.Name.Substring(1)}";
        //         if (schema.Properties.ContainsKey(propName)) {
        //             
        //         }
        //     }   
        // }
    }
}