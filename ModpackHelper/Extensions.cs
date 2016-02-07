using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModpackHelper.Shared
{
    public static class Extensions
    {
        public static bool IsFullyQualifiedUrl(this string input)
        {
            Uri uriResult;
            bool result = Uri.TryCreate(input, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }
    }
}
