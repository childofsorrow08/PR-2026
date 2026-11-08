using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

internal sealed class ParameterDescriptions : IOperationFilter {
    public void Apply(OpenApiOperation operation, OperationFilterContext context) {
        foreach (var parameter in operation.Parameters) {
            if (parameter.Name == "id") {
                parameter.Description = "Цілий ідентифікатор поштового відправлення.";
            }
        }

        if (operation.RequestBody is not null) {
            operation.RequestBody.Description = "JSON із полями відповідної DTO; " +
                "одиниці вимірювання наведені в описі властивостей схеми.";
        }
    }
}
