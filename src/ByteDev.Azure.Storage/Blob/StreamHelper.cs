using System;
using System.IO;
using System.Text;

namespace ByteDev.Azure.Storage.Blob
{
    internal static class StreamHelper
    {
        public static Stream CreateStream(string data)
        {
            return CreateStream(data, Encoding.UTF8);
        }

        public static Stream CreateStream(string data, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(data);
            
            return new MemoryStream(bytes);
        }

        public static string ReadStream(Stream stream)
        {
            return ReadStream(stream, Encoding.UTF8);
        }

        public static string ReadStream(Stream stream, Encoding encoding)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            using (var sr = new StreamReader(stream, encoding))
            {
                return sr.ReadToEnd();
            }
        }
    }
}