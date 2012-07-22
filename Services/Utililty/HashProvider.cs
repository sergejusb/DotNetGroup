namespace DotNetGroup.Services.Utililty
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public interface IHashProvider
    {
        string ComputeHash(string value);
    }

    public class HashProvider : IHashProvider
    {
        private readonly HashAlgorithm hashAlgorithm;

        public HashProvider()
            : this(new MD5CryptoServiceProvider())
        {
        }

        public HashProvider(HashAlgorithm hashAlgorithm)
        {
            this.hashAlgorithm = hashAlgorithm;
        }

        public string ComputeHash(string value)
        {
            var data = this.ComputeByteHash(value);
            var hash = new StringBuilder();
            for (var i = 0; i < data.Length; i++)
            {
                hash.Append(data[i].ToString("x2"));
            }

            return hash.ToString();
        }

        public byte[] ComputeByteHash(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }

            var data = Encoding.Unicode.GetBytes(value);
            return this.hashAlgorithm.ComputeHash(data);
        }
    }
}
