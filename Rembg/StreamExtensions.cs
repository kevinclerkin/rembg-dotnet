

namespace Rembg
{
    public static class StreamExtensions
    {
        public static byte[] ReadAllBytes(this Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
