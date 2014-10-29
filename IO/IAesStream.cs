using System;

namespace MineLib.Network.IO
{
    /// <summary>
    /// Object that implements AES.
    /// </summary>
    public interface IAesStream : IDisposable
    {
        int ReadByte();
        int Read(byte[] buffer, int offset, int count);

        void WriteByte(byte value);
        void Write(byte[] buffer, int offset, int count);

        IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state);
        int EndRead(IAsyncResult asyncResult);

        IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state);
        void EndWrite(IAsyncResult asyncResult);
    }
}