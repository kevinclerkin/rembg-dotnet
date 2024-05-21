using Python.Runtime;


namespace Rembg
{
    public class Remover
    {
        public Stream RemoveBackground(Stream imageStream, string format = "PNG")
        {
            
            PythonEngine.Initialize();

            using (Py.GIL())
            {
                dynamic rembgModule = Py.Import("rembg");
                dynamic ioModule = Py.Import("io");
                dynamic ImageModule = Py.Import("PIL.Image");

                // Read the image stream into a BytesIO object
                PyObject byteStream = ioModule.BytesIO(imageStream.ReadAllBytes());

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
                return new PyStream(outputStream);
            }
        }
    }


}