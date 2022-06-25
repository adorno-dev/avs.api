using System.Text;

namespace AVS.API.Settings
{
    public class TokenSettings
    {
        private static string SecretKey { get; set; } = "fedaf7d8863b48e197b9287d492b708e";

        public static byte[] GetSecret() => Encoding.ASCII.GetBytes(SecretKey);
    }
}