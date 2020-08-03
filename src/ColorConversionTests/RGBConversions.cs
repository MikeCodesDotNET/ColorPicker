using ColorPicker.Structures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ColorConversionTests
{
    [TestClass]
    public class RGBConversions
    {
        [TestMethod]
        public void RGB_To_Int32()
        {
            var cerise = Cerise.Instance;
            var pink = new RGBStruct(cerise.RGB[0], cerise.RGB[1], cerise.RGB[2]);
     
        }
    }
}
