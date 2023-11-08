using System.Security.Cryptography;
using System.Text;

namespace Api.Helpers
{
    public class Md5
    {
        private MD5 _md5;
        public Md5() 
        { 
            _md5 = MD5.Create();
        }

        public string Hash(string value, string pepper)
        {
            if(value == null) throw new ArgumentNullException(nameof(value)); 
            if(pepper == null) throw new ArgumentNullException(nameof(pepper));

            value = ToAscii(value);
            pepper = ToAscii(pepper);

            byte[] inputBytes = Encoding.ASCII.GetBytes(value + pepper);
            byte[] hashBytes = _md5.ComputeHash(inputBytes);

            string lowerHexHash = Convert.ToHexString(hashBytes).ToLower();
            
            return lowerHexHash;
        }

        private string ToAscii(string input)
        {
            // Remove non-ASCII characters
            StringBuilder asciiStringBuilder = new StringBuilder();
            foreach (char c in input)
            {
                if (c <= 127)
                {
                    asciiStringBuilder.Append(c);
                }
            }

            return asciiStringBuilder.ToString();
        }
    }
}
