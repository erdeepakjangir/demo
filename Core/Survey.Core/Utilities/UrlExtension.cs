using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Core.Utilities
{
    public static class UrlExtension
    {
        public static string Combine(params string[] urlParts)
        {
            if (urlParts == null)
                return null;

            var sb = new StringBuilder();
            foreach (var part in urlParts)
            {
                if (string.IsNullOrEmpty(part))
                    continue;
                var trimmedPart = part.TrimEnd('/');
                if (!string.IsNullOrEmpty(trimmedPart))
                {
                    if (sb.Length > 0)
                    {
                        sb.Append("/");
                        trimmedPart = trimmedPart.TrimStart('/');
                    }

                    sb.Append(trimmedPart);
                }
            }
            return sb.ToString();
        }
    }
}
