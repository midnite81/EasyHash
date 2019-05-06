using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Midnite81.EasyHash;
using Midnite81.EasyHash.Model;

namespace Midnite81.EasyHashTests
{
    [TestClass]
    public class HasherTest
    {
        [TestMethod]
        public void ItCreatesAHash()
        {
            var sut = Hasher.MakeHash("password");

            sut.Should().BeOfType<Hash>();
            sut.HashString.Should().BeOfType<string>();
            sut.HashString.Should().HaveLength(32);
            sut.HashBytes.Should().BeOfType<byte[]>();
            sut.HashBytes.Should().HaveCount(24);
            sut.Salt.Bytes.Should().BeOfType<byte[]>();
            sut.Salt.Bytes.Should().HaveCount(24);
        }

        [TestMethod]
        public void ItCreatesASalt()
        {
            var sut = Hasher.GenerateSalt();

            sut.Should().BeOfType<Salt>();
            sut.Bytes.Should().HaveCount(24);
            sut.String.Should().BeOfType<string>();
        }

        [TestMethod]
        public void ItVerifiesHash()
        {
            var original = Hasher.MakeHash("password");

            var sut = Hasher.VerifyHash("password", original.Salt.Bytes, original.HashBytes);

            sut.Should().BeTrue();
        }

        [TestMethod]
        public void ItReturnsFalseWhenTheNewHashDoesntMatch()
        {
            var original = Hasher.MakeHash("password");

            var sut = Hasher.VerifyHash("password2", original.Salt.Bytes, original.HashBytes);

            sut.Should().BeFalse();
        }
    }
}