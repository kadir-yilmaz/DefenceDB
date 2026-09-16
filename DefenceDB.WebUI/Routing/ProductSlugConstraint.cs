using DefenceDB.EL.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DefenceDB.WebUI.Routing;

public class ProductSlugConstraint : IRouteConstraint
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (values.TryGetValue(routeKey, out var value) && value is string slug)
        {
            // Only match if the slug contains the product separator "-p-"
            if (slug.Contains(ProductSlugHelper.Separator, StringComparison.OrdinalIgnoreCase))
            {
                return ProductSlugHelper.TryExtractId(slug, out _);
            }
        }

        return false;
    }
}
