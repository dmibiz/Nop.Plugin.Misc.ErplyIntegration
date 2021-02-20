namespace Nop.Plugin.Misc.ErplyIntegration.Api.Models.Request
{
    /// <summary>
    /// See https://learn-api.erply.com/requests/verifyuser for fields descriptions
    /// </summary>
    public class ErplyVerifyUserRequest : ErplyRequest
    {
        public override string Request { get; } = "verifyUser";

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
