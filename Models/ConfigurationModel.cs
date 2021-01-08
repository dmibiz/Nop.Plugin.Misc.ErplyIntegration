using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
namespace Nop.Plugin.Misc.ErplyIntegration.Models
{
    /// <summary>
    /// Represents plugin configuration model
    /// </summary>
    public class ConfigurationModel : BaseNopModel
    {
        [NopResourceDisplayName("Plugins.Misc.ErplyIntegration.Username")]
        public string Username { get; set; }

        [NopResourceDisplayName("Plugins.Misc.ErplyIntegration.Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NopResourceDisplayName("Plugins.Misc.ErplyIntegration.ClientCode")]
        public string ClientCode { get; set; }
    }
}
