using System;
using System.IO;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

namespace MineLib.Network.IO
{
    public sealed class BouncyAesStream : IDisposable
    {
        public Stream BaseStream { get; private set; }

        private readonly BufferedBlockCipher _decryptCipher;
        private readonly BufferedBlockCipher _encryptCipher;

        private bool _disposed;

        public BouncyAesStream(Stream stream, byte[] key)
        {
            BaseStream = stream;

            _encryptCipher = new BufferedBlockCipher(new CfbBlockCipher(new AesFastEngine(), 8));
            _encryptCipher.Init(true, new ParametersWithIV(new KeyParameter(key), key, 0, 16));

            _decryptCipher = new BufferedBlockCipher(new CfbBlockCipher(new AesFastEngine(), 8));
            _decryptCipher.Init(false, new ParametersWithIV(new KeyParameter(key), key, 0, 16));
        }

        public int ReadByte()
        {
            var value = BaseStream.ReadByte();
            return value == -1 ? value : _decryptCipher.ProcessByte((byte) value)[0];
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            var length = BaseStream.Read(buffer, offset, count);
            var decrypted = _decryptCipher.ProcessBytes(buffer, offset, length);
            Buffer.BlockCopy(decrypted, 0, buffer, offset, decrypted.Length);
            return length;
        }

        public IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return BaseStream.BeginRead(buffer, offset, count, callback, state);
        }

        public int EndRead(IAsyncResult asyncResult)
        {
            return BaseStream.EndRead(asyncResult);
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            var encrypted = _encryptCipher.ProcessBytes(buffer, offset, count);
            BaseStream.Write(encrypted, 0, encrypted.Length);
        }

        public IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            var encrypted = _encryptCipher.ProcessBytes(buffer, offset, count);
            return BaseStream.BeginWrite(encrypted, 0, encrypted.Length, callback, state);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                if (_decryptCipher != null)
                    _decryptCipher.Reset();

                if (_encryptCipher != null)
                    _encryptCipher.Reset();

                if (BaseStream != null)
                    BaseStream.Dispose();
            }

            _disposed = true;
        }

        ~BouncyAesStream()
        {
            Dispose(false);
        }
    }
}
