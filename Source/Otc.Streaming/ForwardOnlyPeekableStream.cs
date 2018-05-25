using System;
using System.IO;

namespace Otc.Streaming
{
    /// <summary>
    /// Forward only, read only stream wrapper with peek capability.
    /// <para>
    /// The purpose of this stream is to wrap a read only, forward-only stream and provides 
    /// capability of peek a amount of data from the begining of stream and virtually not affect the 
    /// stream position.
    /// </para>
    /// </summary>
    public class ForwardOnlyPeekableStream : Stream
    {
        private readonly Stream sourceStream;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="sourceStream">The Forward only, read only stream to wrap</param>
        public ForwardOnlyPeekableStream(Stream sourceStream)
        {
            this.sourceStream = sourceStream ?? throw new ArgumentNullException(nameof(sourceStream));
        }

        /// <inheritdoc />
        public override bool CanRead => true;

        /// <inheritdoc />
        public override bool CanSeek => false;

        /// <inheritdoc />
        public override bool CanWrite => false;

        /// <inheritdoc />
        public override long Length => sourceStream.Length;

        /// <inheritdoc />
        public override long Position
        {
            get
            {
                return ShouldHandleAsRegularStream() ? sourceStream.Position : peekBufferPosition;
            }
            set => throw new NotImplementedException();
        }

        private bool ShouldHandleAsRegularStream()
        {
            return !peeked || peekBuffer == null;
        }

        /// <inheritdoc />
        public override int Read(byte[] buffer, int offset, int count)
        {
            int bytesRead;

            if (ShouldHandleAsRegularStream())
            {
                bytesRead = sourceStream.Read(buffer, offset, count);
            }
            else
            {
                bytesRead = 0;

                while (bytesRead < count && peekBufferPosition < sourceStream.Position
                    && peekBufferPosition < peekBytesRead)
                {
                    buffer[offset + bytesRead] = peekBuffer[peekBufferPosition];
                    peekBufferPosition++;
                    bytesRead++;
                }

                if (peekBufferPosition == sourceStream.Position) // o peekBuffer foi lido por completo
                {
                    peekBuffer = null;
                    bytesRead += sourceStream.Read(buffer, offset + bytesRead, count - bytesRead);
                }
            }
            
            return bytesRead;
        }

        private byte[] peekBuffer;
        private int peekBufferLength;
        private int peekBufferPosition = 0;
        private bool peeked = false;
        private int peekBytesRead;

        /// <summary>
        /// Peek amount of data (up to maxLength) from the begining of stream
        /// </summary>
        /// <param name="maxLength">The maxLength to peek</param>
        /// <returns>Byte array with peeked data</returns>
        public byte[] Peek(int maxLength)
        {
            if (Position > 0)
            {
                throw new InvalidOperationException("Could not peek stream because it has ever started reading.");
            }

            if (peeked)
            {
                throw new InvalidOperationException("This stream has ever peeked before. Peek operation can't be done more than once.");
            }

            peeked = true;
            peekBufferLength = maxLength;
            peekBuffer = new byte[maxLength];
            peekBytesRead = sourceStream.Read(peekBuffer, 0, maxLength);

            var result = new byte[peekBytesRead];

            for (int i = 0; i < peekBytesRead; i++)
            {
                result[i] = peekBuffer[i];
            }

            return result;
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                sourceStream.Dispose();
            }

            base.Dispose(disposing);
        }

        #region [ NotImplemented ]
        /// <summary>
        /// NotImplemented
        /// </summary>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// NotImplemented
        /// </summary>
        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// NotImplemented
        /// </summary>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// NotImplemented
        /// </summary>
        public override void Flush()
        {
            throw new NotImplementedException();
        }
        #endregion
    }

}
