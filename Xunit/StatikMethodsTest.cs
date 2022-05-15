using FluentAssertions;
using RENT.Data.Helpers;

namespace Xunit
{
    public class StatikMethodsTest
    {
        [Fact]
        public void Random()
        {
            string unique36 = RandomString.RandString(36);
            string unique25 = RandomString.RandString(25);
            unique36.Should().HaveLength(36);
            unique25.Should().HaveLength(25);
        }


    } 
}