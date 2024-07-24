
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace SharedLibrary;

public class Blockchain
{
    public List<Block> Chain { get; set; } = new();

    public void AddBlock(object data)
    {
        string previousHash = "";
        if (Chain.LastOrDefault() != null)
        {
            previousHash = Chain.LastOrDefault().Hash;
        }
        string json = JsonSerializer.Serialize(data);
        Block newBlock = new(json, previousHash);
        Chain.Add(newBlock);
    }

    public bool IsValid()
    {
        for (int i = 1; i < Chain.Count; i++)
        {
            Block currentBlock = Chain[i];
            Block previousBlock = Chain[i - 1];

            if (currentBlock.Hash != currentBlock.CalculateHash() ||
                currentBlock.PreviousHash != previousBlock.Hash)
            {
                return false;
            }
        }
        return true;
    }

    public string SerializeToJson()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
    }

    public T GetData<T>(int index)
    {
        if (index < 0 || index >= Chain.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
        }
        return JsonSerializer.Deserialize<T>(Chain[index].Data);
    }

    public class Block
    {
        public string Data { get; private set; }
        public string PreviousHash { get; private set; }
        public string Hash { get; private set; }

        public Block(string data, string previousHash)
        {
            Data = data;
            PreviousHash = previousHash;
            Hash = CalculateHash();
        }

        public string CalculateHash()
        {
            string input = $"{Data}-{PreviousHash}";
            byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
            string hash = BitConverter.ToString(hashBytes).Replace("-", "");
            return hash;
        }
    }
}