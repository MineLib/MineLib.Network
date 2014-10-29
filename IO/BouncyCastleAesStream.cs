using System;
using System.IO;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

namespace MineLib.Network.IO
{
    public sealed class BouncyCastleAesStream : IAesStream
    {
        private readonly Stream _baseStream;

        private readonly BufferedBlockCipher _decryptCipher;
        private readonly BufferedBlockCipher _encryptCipher;

        public BouncyCastleAesStream(Stream stream, byte[] key)
        {
            _baseStream = stream;

            _encryptCipher = new BufferedBlockCipher(new CfbBlockCipher(new AesFastEngine(), 8));
            _encryptCipher.Init(true, new ParametersWithIV(new KeyParameter(key), key, 0, 16));

            _decryptCipher = new BufferedBlockCipher(new CfbBlockCipher(new AesFastEngine(), 8));
            _decryptCipher.Init(false, new ParametersWithIV(new KeyParameter(key), key, 0, 16));
        }


        public int ReadByte()
        {
            var value = _baseStream.ReadByte();
            return value == -1 ? value : _decryptCipher.ProcessByte((byte) value)[0];
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            var length = _baseStream.Read(buffer, offset, count);
            var decrypted = _decryptCipher.ProcessBytes(buffer, offset, length);
            Buffer.BlockCopy(decrypted, 0, buffer, offset, decrypted.Length);
            return length;
        }


        public void WriteByte(byte value)
        {
            var encrypted = _encryptCipher.ProcessBytes(new byte[] { value }, 0, 1);
            _baseStream.Write(encrypted, 0, encrypted.Length);  
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            var encrypted = _encryptCipher.ProcessBytes(buffer, offset, count);
            _baseStream.Write(encrypted, 0, encrypted.Length);
        }


        public IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return _baseStream.BeginRead(buffer, offset, count, callback, state);
        }

        public int EndRead(IAsyncResult asyncResult)
        {
            return _baseStream.EndRead(asyncResult);
        }


        public IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            var encrypted = _encryptCipher.ProcessBytes(buffer, offset, count);
            return _baseStream.BeginWrite(encrypted, 0, encrypted.Length, callback, state);
        }

        public void EndWrite(IAsyncResult asyncResult)
        {
            _baseStream.EndWrite(asyncResult);
        }


        public void Dispose()
        {
            if (_decryptCipher != null)
                _decryptCipher.Reset();

            if (_encryptCipher != null)
                _encryptCipher.Reset();

            if (_baseStream != null)
                _baseStream.Dispose();
        }
    }
}
