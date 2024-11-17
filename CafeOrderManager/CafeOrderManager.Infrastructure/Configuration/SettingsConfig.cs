using CafeOrderManager.Infrastructure.Enums;

namespace CafeOrderManager.Infrastructure.Configuration
{
    public class SettingsConfig
    {
        public string JwtSecurityKey { get; set; }
        public string BaseUrl { get; set; }
        public string ForgetPasswordLink { get; set; }
        public string InviteLink { get; set; }
        public string SlackWebHook { get; set; }
        public string GoogleApiKey { get; set; }
        //public EnvironmentEnum Environment { get; set; }
        public string ExpoPushNotificationUrl { get; set; }
    }
}