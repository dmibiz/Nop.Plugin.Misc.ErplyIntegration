using System;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.Misc.ErplyIntegration.Api.Models.Request
{
    public abstract class ErplyRequest
    {
        abstract public string Request { get; }

        public string ClientCode { get; set; }

        public string SessionKey { get; set; }

        public List<KeyValuePair<string, string>> ToKeyValuePairList()
        {
            return GetType()
                .GetProperties()
                .Where(property => property.GetValue(this) != null && property.Name != "ResponseType")
                .Select(property => new KeyValuePair<string, string>(
                    Char.ToLowerInvariant(property.Name[0]) + property.Name.Substring(1), 
                    property.GetValue(this).ToString()
                    ))
                .ToList();
        }
    }
}
