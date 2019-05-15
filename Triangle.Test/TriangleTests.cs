using NUnit.Framework;
using Triangle;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Form1 form = new Form1();
            
            int x = 1;
            int y = 1;
            Assert.AreEqual(x,y);
        }
    }
}