﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary;

public class RsaSigner
{
    public (byte[] publicKey, byte[] privateKey) GenerateKeyPair()
    {
        using RSACryptoServiceProvider rsa = new();
        byte[] publicKey = rsa.ExportRSAPublicKey();
        byte[] privateKey = rsa.ExportRSAPrivateKey();

        return (publicKey, privateKey);
    }

    public byte[] Sign(byte[] privateKey, byte[] data)
    {
        using RSACryptoServiceProvider rsa = new();
        rsa.ImportRSAPrivateKey(privateKey, out _);
        return rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }

    public bool Verify(byte[] publicKey, byte[] signature, byte[] data)
    {
        try
        {
            using RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportRSAPublicKey(publicKey, out _);
            return rsa.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
        catch (CryptographicException)
        {
            return false;
        }
    }
}