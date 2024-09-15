using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FakeItEasy;
using Rembg;
using Python.Runtime;

namespace RembgTest
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
