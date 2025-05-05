using Microsoft.AspNetCore.DataProtection;

namespace Youth_Innovation_System.Helpers
{
    public static class EncryptionHelper
    {
        private static IDataProtector _protector;

        static EncryptionHelper()
        {
            var dataProtectionProvider = DataProtectionProvider.Create(
               new DirectoryInfo(@"./keys"),
               config => config.SetApplicationName("YouthInnovationSystem")
           );
            _protector = dataProtectionProvider.CreateProtector("PostDescriptionProtector");
        }

        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return plainText;
            return _protector.Protect(plainText);
        }

        public static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText)) return cipherText;
            var result = _protector.Unprotect(cipherText);
            return result;
        }
    }

}
