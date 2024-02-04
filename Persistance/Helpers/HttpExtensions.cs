using System.Web;

namespace Persistence.Helpers
{
    // Static class providing extension methods for working with URIs and query parameters.
    public static class HttpExtensions
    {
        // Extension method to add or replace a query parameter in the URI.
        public static Uri AddQuery(this Uri uri, string name, string value)
        {
            // Parse the existing query parameters.
            var httpValueCollection = HttpUtility.ParseQueryString(uri.Query);

            // Remove the existing parameter with the same name.
            httpValueCollection.Remove(name);

            // Add the new parameter.
            httpValueCollection.Add(name, value);

            // Build a new URI with the modified query parameters.
            var ub = new UriBuilder(uri)
            {
                Query = httpValueCollection.ToString() ?? string.Empty
            };

            return ub.Uri;
        }
    }
}