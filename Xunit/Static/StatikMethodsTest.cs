using FluentAssertions;
using FluentAssertions.Extensions;
using RENT.Data.Helpers;

namespace Rent.Xunit.Static
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
        [Fact]
        public void DateOffsetChack()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(1000);
            long unixTimeStampInSeconds = dateTimeOffset.ToUnixTimeSeconds();
            var expDate = UnixTimeStamp.UnixTimeStampToDateTime(unixTimeStampInSeconds);

            expDate.Should().BeMoreThan(1.Days()).Before(DateTime.UtcNow);
        }
    }
}