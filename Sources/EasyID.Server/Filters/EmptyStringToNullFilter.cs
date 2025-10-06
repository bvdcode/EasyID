using Microsoft.AspNetCore.Mvc.Filters;

namespace EasyID.Server.Filters
{
    /// <summary>
    /// Normalizes empty strings to null in request DTOs to maintain consistency.
    /// </summary>
    public class EmptyStringToNullFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            foreach (var arg in context.ActionArguments.Values)
            {
                if (arg != null)
                {
                    NormalizeProperties(arg);
                }
            }

            await next();
        }

        private static void NormalizeProperties(object obj)
        {
            var type = obj.GetType();
            if (type.IsPrimitive || type == typeof(string) || type.IsValueType)
            {
                return;
            }

            var properties = type.GetProperties()
                .Where(p => p.CanWrite && p.PropertyType == typeof(string));

            foreach (var property in properties)
            {
                var value = property.GetValue(obj) as string;
                if (string.IsNullOrWhiteSpace(value))
                {
                    property.SetValue(obj, null);
                }
            }
        }
    }
}
