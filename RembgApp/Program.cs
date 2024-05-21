using Rembg;

class Program
{
    static void Main(string[] args)
    {
        // Set environment variable
        var pythonHome = @"C:\Users\Kevin\AppData\Local\Programs\Python\Python38";
        var pythonDll = Path.Combine(pythonHome, "python38.dll");
        Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", pythonDll);

        // Define input and output image paths
        string inputImagePath = @"C:\Users\Kevin\Desktop\gym-review-4.png";
        string outputImagePath = @"C:\Users\Kevin\Desktop\gym-review-6.png";

        // Read input image as a stream
        using (var inputImageStream = File.OpenRead(inputImagePath))
        {
            // Create an instance of the Remover class
            var remover = new Remover();

            // Remove background and get the resulting stream
            using (var resultStream = remover.RemoveBackground(inputImageStream))
            {
                // Save the resulting image stream to an output file
                using (var outputFileStream = File.Create(outputImagePath))
                {
                    resultStream.CopyTo(outputFileStream);
                }
            }
        }

        Console.WriteLine("Background removal completed. Result saved to: " + outputImagePath);
    }
}