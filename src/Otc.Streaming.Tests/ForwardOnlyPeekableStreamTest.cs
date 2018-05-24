using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Otc.Streaming.Tests
{
    public class ForwardOnlyPeekableStreamTest
    {
        [Theory]
        [InlineData("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus eu suscipit libero. Nam accumsan velit a sapien rutrum, ac placerat augue pulvinar. Nam eget risus vitae orci faucibus placerat. Nunc gravida tortor in enim venenatis, id volutpat enim varius. Integer nec neque vel felis maximus lacinia. In condimentum risus non magna vehicula ultricies. Nulla bibendum ut quam nec faucibus. Nunc iaculis aliquam lorem ac lobortis. Duis vestibulum et nisi sed sodales. In eget magna lacus. Mauris accumsan purus cursus ante vehicula semper. Nunc in odio congue, dapibus ex ut, blandit ipsum. Etiam condimentum metus vel mauris blandit viverra. Vestibulum dui felis, congue et lectus id, dictum volutpat augue. Duis quis augue id nulla porttitor vulputate vel id tortor. Sed iaculis nisi nunc, in blandit nulla molestie ut. In euismod aliquam velit, at interdum eros condimentum non. Mauris eu facilisis lorem. Etiam placerat efficitur massa a aliquet. Mauris sed augue quis dui laoreet accumsan. Morbi dapibus sapien nec amet.")]
        [InlineData("abc")]
        public void TestPeek(string data)
        {
            var sourceStream = new MemoryStream(Encoding.ASCII.GetBytes(data));
            var peekableStream = new ForwardOnlyPeekableStream(sourceStream);
            int peekSize = 256;
            var peekedBytes = peekableStream.Peek(peekSize);
            var peekedString = Encoding.ASCII.GetString(peekedBytes);
            var testData = data.Length > peekSize ? data.Substring(0, peekSize) : data;
            Assert.Equal(testData, peekedString);
        }

        [Theory]
        [InlineData(1024, 239, 44)]
        [InlineData(44, 239, 1024)]
        [InlineData(102400, 431, 1024)]
        [InlineData(1024, 644, 4096)]
        [InlineData(1111, 2000, 144)]
        [InlineData(256, 128, 128)]
        public void TestPartialRead(int dataLength, int peekSize, int bufferSize)
        {
            var data = new byte[dataLength];

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (byte)i;
            }

            var sourceStream = new MemoryStream(data);
            var peekableStream = new ForwardOnlyPeekableStream(sourceStream);
            var peekedBytes = peekableStream.Peek(peekSize);

            for (int i = 0; i < peekedBytes.Length; i++)
            {
                Assert.Equal((byte)i, peekedBytes[i]);
            }

            var buffer = new byte[bufferSize];
            int readBytes;
            var testList = new List<byte>();

            while ((readBytes = peekableStream.Read(buffer, 0, bufferSize)) > 0)
            {
                for (int i = 0; i < readBytes; i++)
                {
                    testList.Add(buffer[i]);
                }
            }

            for (int i = 0; i < dataLength; i++)
            {
                Assert.Equal(data[i], testList[i]);
            }
        }
    }

}

