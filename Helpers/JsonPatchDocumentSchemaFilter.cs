using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HarvestCore.WebApi.Helpers
{
    /// <summary>
    /// Un filtro de esquema para Swagger que simplifica la apariencia de JsonPatchDocument.
    /// Oculta las propiedades 'operationType' y 'from' que no se usan comúnmente,
    /// haciendo que el cuerpo de la solicitud en la UI de Swagger sea más limpio.
    /// </summary>
    public class JsonPatchDocumentSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            // El filtro se aplica al tipo 'Operation', que es el objeto dentro del array de JsonPatchDocument.
            if (context.Type != typeof(Operation))
            {
                return;
            }

            // Elimina las propiedades que no queremos mostrar en la UI de Swagger.
            schema.Properties.Remove("operationType"); // Representación numérica interna de 'op'.
            schema.Properties.Remove("from"); // Solo se usa para operaciones 'move' y 'copy'.
        }
    }
}
