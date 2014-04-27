using System.IO;
using System.Security.Cryptography;

namespace MineLib.Network.IO
{
    // -- Credits to umby24 for encryption support, as taken from CWrapped.
    public class AesStream
    {
        public readonly CryptoStream DecryptStream;
        public readonly CryptoStream EncryptStream;
        private readonly Stream _baseStream;

        public AesStream(Stream stream, byte[] key)
        {
            _baseStream = stream;

            var raj = GenerateAES(key);
            var encTrans = raj.CreateEncryptor();
            var decTrans = raj.CreateDecryptor();

            EncryptStream = new CryptoStream(_baseStream, encTrans, CryptoStreamMode.Write);
            DecryptStream = new CryptoStream(_baseStream, decTrans, CryptoStreamMode.Read);
        }

        public int ReadByte()
        {
            return DecryptStream.ReadByte();
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            return DecryptStream.Read(buffer, offset, count);
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            EncryptStream.Write(buffer, offset, count);
        }

        private Rijndael GenerateAES(byte[] key)
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
            if (DecryptStream != null)
                DecryptStream.Dispose();

            if (EncryptStream != null)
                EncryptStream.Dispose();

            if (_baseStream != null)
                _baseStream.Dispose();
        }
    }
}
