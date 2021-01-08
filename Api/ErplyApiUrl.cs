
namespace Nop.Plugin.Misc.ErplyIntegration.Api
{
    class ErplyApiUrl
    {
        #region Fields

        public string ClientCode { get; set; }

        #endregion

        #region Methods

        public override string ToString()
        {
            return $"https://{ClientCode}.erply.com/api/";
        }
        #endregion
    }
}
