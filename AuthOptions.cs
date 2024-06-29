using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace backend
{
    public class AuthOptions
    {
        public const string ISSUER = "CollectionsAPI";
        public const string AUDIENCE = "CollectionsClient";

        private const string KEY = "ddwgrwdaAWFSRGDzfawfgsraAKLJFNGKJRSGNDFKJsmdwackeGNSSJKWDMAKDMCVrJKLfe";

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
        }
    }
}
