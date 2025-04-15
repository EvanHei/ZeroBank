using SharedLibrary;
using System;
using System.Text;
using Xunit;

public class RsaSignerTests
{
    private readonly RsaSigner _rsaSigner;

    public RsaSignerTests()
    {
        _rsaSigner = new RsaSigner();
    }

    [Fact]
    public void GenerateKeyPair_ShouldGeneratePublicAndPrivateKey()
    {
        // Act
        var (publicKey, privateKey) = _rsaSigner.GenerateKeyPair();

        // Assert
        Assert.NotNull(publicKey);
        Assert.NotNull(privateKey);
        Assert.True(publicKey.Length > 0);
        Assert.True(privateKey.Length > 0);
    }

    [Fact]
    public void Sign_ValidData_ShouldReturnValidSignature()
    {
        // Arrange
        var (publicKey, privateKey) = _rsaSigner.GenerateKeyPair();
        byte[] data = Encoding.UTF8.GetBytes("Test data");

        // Act
        byte[] signature = _rsaSigner.Sign(privateKey, data);

        // Assert
        Assert.NotNull(signature);
        Assert.True(signature.Length > 0);
    }

    [Fact]
    public void Verify_ValidSignature_ShouldReturnTrue()
    {
        // Arrange
        var (publicKey, privateKey) = _rsaSigner.GenerateKeyPair();
        byte[] data = Encoding.UTF8.GetBytes("Test data");
        byte[] signature = _rsaSigner.Sign(privateKey, data);

        // Act
        bool isValid = _rsaSigner.Verify(publicKey, signature, data);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void Verify_InvalidSignature_ShouldReturnFalse()
    {
        // Arrange
        var (publicKey, privateKey) = _rsaSigner.GenerateKeyPair();
        byte[] data = Encoding.UTF8.GetBytes("Test data");
        byte[] signature = _rsaSigner.Sign(privateKey, data);
        byte[] alteredData = Encoding.UTF8.GetBytes("Altered data");

        // Act
        bool isValid = _rsaSigner.Verify(publicKey, signature, alteredData);

        // Assert
        Assert.False(isValid);
    }
}
