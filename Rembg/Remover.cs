using Python.Runtime;


namespace Rembg
{
    public class Remover : IDisposable
    {
        private bool _disposed = false;
        private readonly IntPtr _threadState;

        public Remover()
        {
            // Ensure PythonEngine is initialized only once
            if (!PythonEngine.IsInitialized)
            {
                PythonEngine.Initialize();
                _threadState = PythonEngine.BeginAllowThreads();
            }
        }

        public Stream RemoveBackground(Stream imageStream, string format = "PNG")
        {
            if (imageStream == null || !imageStream.CanRead)
                throw new ArgumentException("Invalid image stream");


            using (Py.GIL())
            {
                try
                {
                    dynamic rembgModule = Py.Import("rembg");
                    dynamic ioModule = Py.Import("io");
                    dynamic ImageModule = Py.Import("PIL.Image");

                    using (var byteStream = ioModule.BytesIO(ReadAllBytes(imageStream)))
                    {
                        dynamic inputImage = ImageModule.open(byteStream);
                        dynamic outputImage = rembgModule.remove(inputImage);

                        using (var outputStream = ioModule.BytesIO())
                        {
                            outputImage.save(outputStream, format);
                            outputStream.GetAttr("seek").Invoke(new PyInt(0));
                            return new PyStream(outputStream);
                        }
                    }
                }
                catch (PythonException ex)
                {
                    throw new InvalidOperationException("Failed to remove background", ex);
                }
            }


        }

        private byte[] ReadAllBytes(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }

        protected virtual void Dispose(bool disposing)
        {

            if (!_disposed)
            {
                if (disposing)
                {
                    if (PythonEngine.IsInitialized)
                    {
                        using (Py.GIL())
                        {
                            PythonEngine.Shutdown();
                        }
                    }
                }
                _disposed = true;
            }
        }

    }

}