using Python.Runtime;


namespace Rembg
{
    public class Remover
    {
        public Stream RemoveBackground(Stream imageStream, string format = "PNG")
        {
            var pythonHome = @"C:\Users\Kevin\AppData\Local\Programs\Python\Python38";
            var pythonDll = Path.Combine(pythonHome, "python38.dll");
            Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", pythonDll);
            
            PythonEngine.Initialize();

            using (Py.GIL())
            {
                dynamic rembgModule = Py.Import("rembg");
                dynamic ioModule = Py.Import("io");
                dynamic ImageModule = Py.Import("PIL.Image");

                // Read the image stream into a BytesIO object
                PyObject byteStream = ioModule.BytesIO();

                // Open the image using PIL
                dynamic inputImage = ImageModule.open(byteStream);

                // Remove the background
                dynamic outputImage = rembgModule.remove(inputImage);

                // Save the result to a BytesIO stream
                PyObject outputStream = ioModule.BytesIO();
                outputImage.save(outputStream, format);

                // Reset the stream position to the beginning
                outputStream.GetAttr("seek").Invoke(new PyInt(0));

                // Get the resulting stream as a .NET Stream
                //return new PyStream(outputStream);

                return null!;
            }
        }
    }


    public class PyStream : Stream
    {
        public override bool CanRead => throw new NotImplementedException();

        public override bool CanSeek => throw new NotImplementedException();

        public override bool CanWrite => throw new NotImplementedException();

        public override long Length => throw new NotImplementedException();

        public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }


}