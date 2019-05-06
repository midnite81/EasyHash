using System;
using System.Security.Cryptography;
using Midnite81.EasyHash.Contracts;
using Midnite81.EasyHash.Model;

namespace Midnite81.EasyHash
{
    public class Hasher : IHasher
    {
        public Hash MakeHash(
            string stringToBeHashed,
            byte[] salt = default,
            int iterations = 100000,
            int hashByteSize = 24)
        {
            if (salt == default)
            {
                salt = GenerateSalt().Bytes;
            }

            using (var generator = new Rfc2898DeriveBytes(stringToBeHashed, salt) {IterationCount = iterations})
            {
                var bytes = generator.GetBytes(hashByteSize);
                return new Hash
                {
                    HashString = Convert.ToBase64String(bytes),
                    HashBytes = bytes,
                    Salt = new Salt
                    {
                        Bytes = salt,
                        String = Convert.ToBase64String(salt)
                    }
                };
            }
        }

        public byte[] ConvertStringToBytes(string hashString)
        {
            return Convert.FromBase64String(hashString);
        }

        public Salt GenerateSalt(int saltByteSize = 24)
        {
            using (var saltGenerator = new RNGCryptoServiceProvider())
            {
                var salt = new byte[saltByteSize];
                saltGenerator.GetBytes(salt);
                return new Salt
                {
                    Bytes = salt,
                    String = Convert.ToBase64String(salt)
                };
            }
        }

        public bool VerifyHash(string stringToVerify, byte[] passwordSalt, byte[] passwordHash)
        {
            var computedHash = MakeHash(stringToVerify, passwordSalt).HashBytes;
            return CheckHashes(computedHash, passwordHash);
        }

        private bool CheckHashes(byte[] firstHash, byte[] secondHash)
        {
            var minHashLength = firstHash.Length <= secondHash.Length ? firstHash.Length : secondHash.Length;
            var xor = firstHash.Length ^ secondHash.Length;
            for (var i = 0; i < minHashLength; i++)
                xor |= firstHash[i] ^ secondHash[i];
            return 0 == xor;
        }
    }
}