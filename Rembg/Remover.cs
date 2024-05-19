using Python.Runtime;


namespace Rembg
{
    public class Remover
    {
        public byte[] RemoveBackground(byte[] imageBytes)
        {
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic rembg = Py.Import("rembg");
                dynamic image = Py.Import("PIL.Image");
                using (var inputStream = new MemoryStream(imageBytes))
                using (var outputStream = new MemoryStream())
                {
                    dynamic inputImage = image.open(inputStream);
                    dynamic outputImage = rembg.remove(inputImage);
                    outputImage.save(outputStream, "PNG");
                    return outputStream.ToArray();
                }
            }
        }


    }

    
}