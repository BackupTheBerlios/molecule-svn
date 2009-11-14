using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Compression;
using System.IO;
using Mono.Rocks;

namespace Molecule
{
    public static class StringExtensions
    {
        public static string Zip(this string decompressed)
        {
            return internalZip(decompressed, CompressionMode.Compress);
        }

        public static string UnZip(this string compressed)
        {
            return internalZip(compressed, CompressionMode.Decompress);
        }

        private static string internalZip(string input, CompressionMode mode)
        {
            using (var inputStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(input)))
            using (var compressStream = new DeflateStream(inputStream, CompressionMode.Compress, false))
            using (var outputStream = new MemoryStream()) {
                compressStream.WriteTo(outputStream);
                return Convert.ToBase64String(outputStream.ToArray());
            }
        }

        
    }
}
