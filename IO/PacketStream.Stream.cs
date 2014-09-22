using System;
using System.IO;

namespace MineLib.Network.IO
{
    public partial class PacketStream : Stream
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
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
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
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return EncryptionEnabled ? _crypto.BaseStream.CanWrite : _stream.CanWrite; }
        }

        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        public override long Position 
        {
            get
            {
                if (EncryptionEnabled) 
                    throw new NotSupportedException();
                return _stream.Position;
            }
            set
            {
                if (EncryptionEnabled)
                    throw new NotSupportedException();
                _stream.Position = value;
            }
        }
    }
}
