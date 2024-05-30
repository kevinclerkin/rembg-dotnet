using Python.Runtime;


namespace Rembg
{
    public class PyStream : Stream
    {
        private readonly PyObject _pyStream;
        private readonly PyObject _readMethod;
        private int _position;
        private bool _disposed = false;
        private readonly object _lock = new object();

        public PyStream(PyObject pyStream)
        {
            _pyStream = pyStream ?? throw new ArgumentNullException(nameof(pyStream));
            _readMethod = _pyStream.GetAttr("read") ?? throw new InvalidOperationException("The Python stream does not have a 'read' method.");
            _position = 0;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(PyStream));

            lock (_lock) // Ensure this block is not re-entered
            {
                try
                {
                    byte[] bytes;

                    using (Py.GIL())
                    {
                        PyObject result = _readMethod.Invoke(new PyInt(count));
                        bytes = result.As<byte[]>();
                    }

                    Array.Copy(bytes, 0, buffer, offset, bytes.Length);
                    _position += bytes.Length;

                    return bytes.Length;
                }
                catch (PythonException ex)
                {
                    throw new InvalidOperationException("Failed to read from the Python stream", ex);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("An unexpected error occurred while reading from the stream", ex);
                }
            }
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

        protected override void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                using (Py.GIL())
                {
                    _pyStream?.Dispose();
                }
            }

            _disposed = true;
            base.Dispose(disposing);
        }
    }
}
