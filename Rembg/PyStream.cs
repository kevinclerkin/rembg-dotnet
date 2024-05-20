using Python.Runtime;


namespace Rembg
{
    public class PyStream : Stream
    {
        private readonly PyObject _pyStream;
        private readonly PyObject _readMethod;
        private int _position;

        public PyStream(PyObject pyStream)
        {
            _pyStream = pyStream;
            _readMethod = _pyStream.GetAttr("read");
            _position = 0;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            PyObject result = _readMethod.Invoke(new PyInt(count));
            byte[] bytes = result.As<byte[]>();
            Array.Copy(bytes, 0, buffer, offset, bytes.Length);
            _position += bytes.Length;
            return bytes.Length;
        }

        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException();
        public override long Position
        {
            get => _position;
            set => throw new NotSupportedException();
        }

        public override void Flush() => throw new NotSupportedException();
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

    }
}
