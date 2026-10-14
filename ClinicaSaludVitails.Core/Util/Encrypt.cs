
using System.Security.Cryptography;
using System.Text;

namespace ClinicaSaludVitails.Core.Util
{
    public static class Encrypt
    {
        public static string GetSHA256(string str)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] stream = sha256.ComputeHash(Encoding.UTF8.GetBytes(str));
                StringBuilder sb = new StringBuilder();

                foreach (byte b in stream)
                {
                    sb.AppendFormat("{0:x2}", b);
                }

                return sb.ToString();
            }
        }
    }
}
