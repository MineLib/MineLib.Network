using System;
using System.IO;
using System.Security.Cryptography;

namespace MineLib.Network.IO
{    
    // -- Credits to umby24 for encryption support, as taken from CWrapped.
    public sealed class NativeAesStream : IAesStream
    {
        private readonly Stream _baseStream;

        private readonly CryptoStream _decryptStream;
        private readonly CryptoStream _encryptStream;

        public NativeAesStream(Stream stream, byte[] key)
        {
            _baseStream = stream;

            var raj = GenerateAes(key);
            var encTrans = raj.CreateEncryptor();
            var decTrans = raj.CreateDecryptor();

            _encryptStream = new CryptoStream(_baseStream, encTrans, CryptoStreamMode.Write);
            _decryptStream = new CryptoStream(_baseStream, decTrans, CryptoStreamMode.Read);
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


        public int ReadByte()
        {
            return _decryptStream.ReadByte();
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            return _decryptStream.Read(buffer, offset, count);
        }


        public void WriteByte(byte value)
        {
            _encryptStream.WriteByte(value);
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            _encryptStream.Write(buffer, offset, count);
        }


        public IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return _decryptStream.BeginRead(buffer, offset, count, callback, state);
        }

        public int EndRead(IAsyncResult asyncResult)
        {
            return _decryptStream.EndRead(asyncResult);
        }


        public IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return _encryptStream.BeginWrite(buffer, offset, count, callback, state);
        }

        public void EndWrite(IAsyncResult asyncResult)
        {
            _encryptStream.EndWrite(asyncResult);
        }


        public void Dispose()
        {
            if (_decryptStream != null)
                _decryptStream.Dispose();

            if (_encryptStream != null)
                _encryptStream.Dispose();

            if (_baseStream != null)
                _baseStream.Dispose();
        }
    }
}