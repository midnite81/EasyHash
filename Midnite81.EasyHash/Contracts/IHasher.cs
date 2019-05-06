using Midnite81.EasyHash.Model;

namespace Midnite81.EasyHash.Contracts
{
    public interface IHasher
    {
        Hash MakeHash(
            string stringToBeHashed,
            byte[] salt = default,
            int iterations = 100000,
            int hashByteSize = 24);

        Salt GenerateSalt(int saltByteSize = 24);
        bool VerifyHash(string stringToVerify, byte[] passwordSalt, byte[] passwordHash);
    }
}