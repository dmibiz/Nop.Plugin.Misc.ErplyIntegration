using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Configuration;

namespace Nop.Plugin.Misc.ErplyIntegration
{
    public class ErplyIntegrationSettings : ISettings
    {
        /// <summary>
        /// Gets or sets Erply account username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets Erply account password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets Erply account client code
        /// </summary>
        public string ClientCode { get; set; }
    }
}
