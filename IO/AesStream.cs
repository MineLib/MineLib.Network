using System;
using System.IO;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

namespace MineLib.Network.IO
{
    public interface IAesStream
    {
        Stream BaseStream { get; set; }

        int ReadByte();
        int Read(byte[] myBytes, int p1, int value);

        void Write(byte[] tempBuff, int i, int length);

        void Dispose();
    }

    // Should be deleted later. I'll keep it for some time.
    // -- Credits to umby24 for encryption support, as taken from CWrapped.
    public class NativeAesStream : IAesStream
    {
        public Stream BaseStream { get; set; }

        private readonly CryptoStream _decryptStream;
        private readonly CryptoStream _encryptStream;

        public NativeAesStream(Stream stream, byte[] key)
        {
            BaseStream = stream;

            var raj = GenerateAes(key);
            var encTrans = raj.CreateEncryptor();
            var decTrans = raj.CreateDecryptor();

            _encryptStream = new CryptoStream(BaseStream, encTrans, CryptoStreamMode.Write);
            _decryptStream = new CryptoStream(BaseStream, decTrans, CryptoStreamMode.Read);
        }

        public int ReadByte()
        {
            return _decryptStream.ReadByte();
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            return _decryptStream.Read(buffer, offset, count);
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            _encryptStream.Write(buffer, offset, count);
        }

        private static Rijndael GenerateAes(byte[] key)
        {
            var cipher = new RijndaelManaged
            {
                Mode = CipherMode.CFB,
                Padding = PaddingMode.None,
                KeySize = 128,
                FeedbackSize = 8,
                Key = key,
                IV = key
            };

            return cipher;
        }

        public void Dispose()
        {
            if (_decryptStream != null)
                _decryptStream.Dispose();

            if (_encryptStream != null)
                _encryptStream.Dispose();

            if (BaseStream != null)
                BaseStream.Dispose();
        }
    }

    public class BouncyAesStream : IAesStream
    {
        public Stream BaseStream { get; set; }

        private readonly BufferedBlockCipher _decryptCipher;
        private readonly BufferedBlockCipher _encryptCipher;

        public BouncyAesStream(Stream stream, byte[] key)
        {
            BaseStream = stream;

            _encryptCipher = new BufferedBlockCipher(new CfbBlockCipher(new AesFastEngine(), 8));
            _encryptCipher.Init(true, new ParametersWithIV(
                new KeyParameter(key), key, 0, 16));
            _decryptCipher = new BufferedBlockCipher(new CfbBlockCipher(new AesFastEngine(), 8));
            _decryptCipher.Init(false, new ParametersWithIV(
                new KeyParameter(key), key, 0, 16));
        }

        public int ReadByte()
        {
            var value = BaseStream.ReadByte();
            return value == -1 ? value : _decryptCipher.ProcessByte((byte)value)[0];
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            var length = BaseStream.Read(buffer, offset, count);
            var decrypted = _decryptCipher.ProcessBytes(buffer, offset, length);
            Array.Copy(decrypted, 0, buffer, offset, decrypted.Length);
            return length;
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            var encrypted = _encryptCipher.ProcessBytes(buffer, offset, count);
            BaseStream.Write(encrypted, 0, encrypted.Length);
        }

        public void Dispose()
        {
            if (_decryptCipher != null)
                _decryptCipher.Reset();

            if (_encryptCipher != null)
                _encryptCipher.Reset();

            if (BaseStream != null)
                BaseStream.Dispose();
        }
    }
}
