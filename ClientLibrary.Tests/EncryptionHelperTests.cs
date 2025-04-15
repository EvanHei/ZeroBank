using Microsoft.Research.SEAL;
using ClientLibrary;
using System;
using Xunit;

namespace ClientLibrary.Tests;

public class EncryptionHelperTests
{
    private readonly EncryptionHelper _encryptionHelper;
    private readonly SEALContext _SEALcontext;
    private readonly PublicKey _publicKey;
    private readonly SecretKey _secretKey;

    public EncryptionHelperTests()
    {
        _encryptionHelper = new EncryptionHelper();

        EncryptionParameters parms = new(SchemeType.BFV);
        parms.PolyModulusDegree = 4096;
        parms.CoeffModulus = CoeffModulus.BFVDefault(4096);
        parms.PlainModulus = PlainModulus.Batching(parms.PolyModulusDegree, 16);

        _SEALcontext = new SEALContext(parms);
        var keygenResult = _encryptionHelper.GenerateKeys(parms);
        _publicKey = keygenResult.SEALPublicKey;
        _secretKey = keygenResult.SEALSecretKey;
    }

    [Fact]
    public void Encrypt_PositiveNumber_ReturnsCiphertext()
    {
        long input = 123;
        var ciphertext = _encryptionHelper.Encrypt(input, _SEALcontext, _publicKey, _secretKey);

        Assert.NotNull(ciphertext);
        Assert.IsType<Serializable<Ciphertext>>(ciphertext);
    }

    [Fact]
    public void Encrypt_NegativeNumber_StillDecryptsCorrectly()
    {
        long input = -42;

        using MemoryStream ciphertextStream = new();
        _encryptionHelper.Encrypt(input, _SEALcontext, _publicKey, _secretKey).Save(ciphertextStream);

        ciphertextStream.Seek(0, SeekOrigin.Begin);
        Ciphertext loadedCiphertext = new();
        loadedCiphertext.Load(_SEALcontext, ciphertextStream);

        long result = _encryptionHelper.Decrypt(loadedCiphertext, _SEALcontext, _secretKey);

        Assert.Equal(input, result);
    }

    [Fact]
    public void Decrypt_EncryptedPositive_ReturnsOriginal()
    {
        long input = 95;

        using MemoryStream ciphertextStream = new();
        _encryptionHelper.Encrypt(input, _SEALcontext, _publicKey, _secretKey).Save(ciphertextStream);

        ciphertextStream.Seek(0, SeekOrigin.Begin);
        Ciphertext loadedCiphertext = new();
        loadedCiphertext.Load(_SEALcontext, ciphertextStream);

        long result = _encryptionHelper.Decrypt(loadedCiphertext, _SEALcontext, _secretKey);

        Assert.Equal(input, result);
    }

    [Fact]
    public void GenerateKeys_ReturnsValidKeys()
    {
        EncryptionParameters parms = new(SchemeType.BFV)
        {
            PolyModulusDegree = 4096,
            CoeffModulus = CoeffModulus.BFVDefault(4096),
            PlainModulus = new Modulus(1024)
        };

        var result = _encryptionHelper.GenerateKeys(parms);

        Assert.NotNull(result.SEALPublicKey);
        Assert.NotNull(result.SEALSecretKey);
        Assert.NotNull(result.SEALRelinKeys);
        Assert.NotEmpty(result.publicSigningKey);
        Assert.NotEmpty(result.privateSigningKey);
    }

    [Fact]
    public void Encrypt_ThrowsIfNullContext()
    {
        Assert.Throws<ArgumentNullException>(() =>
            _encryptionHelper.Encrypt(1, null, _publicKey, _secretKey));
    }

    [Fact]
    public void Decrypt_ThrowsIfNullContext()
    {
        Ciphertext ciphertext = new();
        Assert.Throws<ArgumentNullException>(() =>
            _encryptionHelper.Decrypt(ciphertext, null, _secretKey));
    }
}
