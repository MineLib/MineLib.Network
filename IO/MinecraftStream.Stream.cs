using System.IO;

namespace MineLib.Network.IO
{
    public sealed partial class MinecraftStream : Stream
    {
        public override void Flush()
        {
            if (EncryptionEnabled)
                _crypto.BaseStream.Flush();
            else
                _stream.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (EncryptionEnabled)
                return _crypto.BaseStream.Seek(offset, origin);
            else
                return _stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            if (EncryptionEnabled)
                _crypto.BaseStream.SetLength(value);
            else
                _stream.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (EncryptionEnabled)
                return _crypto.Read(buffer, offset, count);
            else
                return _stream.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (EncryptionEnabled)
                _crypto.Write(buffer, offset, count);
            else
                _stream.Write(buffer, offset, count);
        }

        public override bool CanRead
        {
            get { return EncryptionEnabled ? _crypto.BaseStream.CanRead : _stream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return EncryptionEnabled ? _crypto.BaseStream.CanSeek : _stream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return EncryptionEnabled ? _crypto.BaseStream.CanWrite : _stream.CanWrite; }
        }

        public override long Length
        {
            get { return EncryptionEnabled ? _crypto.BaseStream.Length : _stream.Length; }
        }

        public override long Position 
        {
            get
            {
                if (EncryptionEnabled)
                    return _crypto.BaseStream.Position;
                else
                    return _stream.Position;
            }
            set
            {
                if (EncryptionEnabled)
                    _crypto.BaseStream.Position = value;
                else
                    _stream.Position = value;
            }
        }
    }
}
