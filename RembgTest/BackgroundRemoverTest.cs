using Rembg;
using Xunit;

namespace RembgTest
{
    public class BackgroundRemoverTest
    {
        public BackgroundRemoverTest()
        {
            // Set the PYTHONNET_PYDLL environment variable to point to the Python DLL

            var pythonHome = @"C:\Users\Kevin\AppData\Local\Programs\Python\Python38";
            var pythonDll = Path.Combine(pythonHome, "python38.dll");
            Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", pythonDll);
        }

        [Fact]
        public void TestToRemoveImageBackground()
        {
            var remover = new Remover();

            using (var imageStream = File.OpenRead(@"C:\Users\Kevin\Desktop\gym-review-4.png"))
            {
                Stream resultStream = remover.RemoveBackground(imageStream);

                Assert.NotNull(resultStream);
                Assert.IsAssignableFrom<Stream>(resultStream);
                Assert.IsType<PyStream>(resultStream);

            }
        }
    }
}