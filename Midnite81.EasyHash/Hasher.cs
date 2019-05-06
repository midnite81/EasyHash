using System;
using System.Security.Cryptography;
using Midnite81.EasyHash.Model;

namespace Midnite81.EasyHash
{
    public static class Hasher
    {
        private const int SaltByteSize = 24;
        private const int HashByteSize = 24;
        private const int HashingIterations = 100000;

        public static Hash MakeHash(
            string stringToBeHashed,
            byte[] salt = default,
            int iterations = HashingIterations,
            int hashByteSize = HashByteSize)
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

        public static Salt GenerateSalt(int saltByteSize = SaltByteSize)
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

        public static bool VerifyHash(string stringToVerify, byte[] passwordSalt, byte[] passwordHash)
        {
            var computedHash = MakeHash(stringToVerify, passwordSalt).HashBytes;
            return CheckHashes(computedHash, passwordHash);
        }

        private static bool CheckHashes(byte[] firstHash, byte[] secondHash)
        {
            var minHashLenght = firstHash.Length <= secondHash.Length ? firstHash.Length : secondHash.Length;
            var xor = firstHash.Length ^ secondHash.Length;
            for (var i = 0; i < minHashLenght; i++)
                xor |= firstHash[i] ^ secondHash[i];
            return 0 == xor;
        }
    }
}