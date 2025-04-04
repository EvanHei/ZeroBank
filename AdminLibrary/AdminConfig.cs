using Microsoft.Research.SEAL;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminLibrary;

public static class AdminConfig
{
    public static ApiAccessor ApiAccessor { get; set; } = new ApiAccessor();

    public static async Task AdminLogin(Credentials adminCredentials)
    {
        await ApiAccessor.AdminLogin(adminCredentials);
    }

    public static async Task AdminCreate(Credentials adminCredentials)
    {
        await ApiAccessor.AdminCreate(adminCredentials);
        await ApiAccessor.AdminLogin(adminCredentials);
    }

    public static async Task AdminDelete(Credentials adminCredentials)
    {
        await ApiAccessor.AdminDelete(adminCredentials);
    }

    public static ulong LoadPlainModulus(Account account)
    {
        using EncryptionParameters parms = new();
        using MemoryStream stream = new(account.Parms);
        parms.Load(stream);
        return parms.PlainModulus.Value;
    }

    public static List<PlaintextTransaction> GetPlaintextTransactions(Account account, string secretKeyString)
    {
        List<PlaintextTransaction> plaintextTransactions = new();

        // load parms
        using EncryptionParameters parms = new();
        using MemoryStream parmsStream = new(account.Parms);
        parms.Load(parmsStream);
        using SEALContext context = new(parms);

        // load decryption key
        byte[] secretKeyBytes = Convert.FromBase64String(secretKeyString);
        using MemoryStream keyStream = new(secretKeyBytes);
        using SecretKey secretKey = new();
        secretKey.Load(context, keyStream);

        foreach (CiphertextTransaction ciphertextTransaction in account.Transactions)
        {
            // load ciphertext
            using MemoryStream memStream = new(ciphertextTransaction.Ciphertext);
            Ciphertext ciphertext = new();
            ciphertext.Load(context, memStream);

            // decrypt
            List<long> result = new();
            PlaintextTransaction plaintextTransaction = new();
            using BatchEncoder encoder = new(context);
            using Plaintext plaintext = new();
            using Decryptor decryptor = new(context, secretKey);
            decryptor.Decrypt(ciphertext, plaintext);
            encoder.Decode(plaintext, result);
            plaintextTransaction.Amount = result[0];

            // initialize and add the new plaintext transaction
            plaintextTransaction.AccountId = ciphertextTransaction.AccountId;
            plaintextTransaction.Timestamp = ciphertextTransaction.Timestamp;
            plaintextTransactions.Add(plaintextTransaction);
        }

        return plaintextTransactions;
    }

    public static string GetFormattedBalance(List<PlaintextTransaction> transactions)
    {
        if (transactions == null || transactions.Count == 0)
        {
            return "$0.00";
        }

        long totalAmount = transactions.Sum(t => t.Amount);

        NumberFormatInfo customFormat = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
        customFormat.CurrencyNegativePattern = 1;

        return (totalAmount * 0.01).ToString("C", customFormat);
    }
}
