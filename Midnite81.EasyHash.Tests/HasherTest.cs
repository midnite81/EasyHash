using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Midnite81.EasyHash;
using Midnite81.EasyHash.Model;

namespace Midnite81.EasyHashTests
{
    [TestClass]
    public class HasherTest
    {
        private Hasher _sut;

        [TestInitialize]
        public void Initialise()
        {
            _sut = new Hasher();
        }

        [TestMethod]
        public void ItCreatesAHash()
        {
            var makeHash = _sut.MakeHash("password");

            makeHash.Should().BeOfType<Hash>();
            makeHash.HashString.Should().BeOfType<string>();
            makeHash.HashString.Should().HaveLength(32);
            makeHash.HashBytes.Should().BeOfType<byte[]>();
            makeHash.HashBytes.Should().HaveCount(24);
            makeHash.Salt.Bytes.Should().BeOfType<byte[]>();
            makeHash.Salt.Bytes.Should().HaveCount(24);
        }

        [TestMethod]
        public void ItCreatesASalt()
        {
            var salt = _sut.GenerateSalt();

            salt.Should().BeOfType<Salt>();
            salt.Bytes.Should().HaveCount(24);
            salt.String.Should().BeOfType<string>();
        }

        [TestMethod]
        public void ItVerifiesHash()
        {
            var original = _sut.MakeHash("password");

            var verifyHash = _sut.VerifyHash("password", original.Salt.Bytes, original.HashBytes);

            verifyHash.Should().BeTrue();
        }

        [TestMethod]
        public void ItReturnsFalseWhenTheNewHashDoesntMatch()
        {
            var original = _sut.MakeHash("password");

            var verifyHash = _sut.VerifyHash("password2", original.Salt.Bytes, original.HashBytes);

            verifyHash.Should().BeFalse();
        }

        [TestMethod]
        public void ItGeneratesBytesFromString()
        {
            var original = _sut.MakeHash("password");

            var convert = _sut.ConvertStringToBytes(original.HashString);

            convert.Should().BeOfType<byte[]>();
            convert.Should().HaveCount(24);
        }
    }
}