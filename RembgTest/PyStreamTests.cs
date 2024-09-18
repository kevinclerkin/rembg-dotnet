using Xunit;
using Rembg;


namespace RembgTests
{
    public class PyStreamTests
    {
        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenPyStreamIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new PyStream(null!));
        }

    }
}
