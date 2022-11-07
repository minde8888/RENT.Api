using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using FluentAssertions.Extensions;
using RENT.Data.Helpers;
using RENT.Data.Interfaces.IServices;
using RENT.Domain.Entities;

namespace Rent.Xunit.Static
{
    public class StaticMethodsTest
    {
        [Fact]
        public void Random()
        {
            var unique36 = RandomString.RandString(36);
            var unique25 = RandomString.RandString(25);
            unique36.Should().HaveLength(36);
            unique25.Should().HaveLength(25);
        }
        [Fact]
        public void DateOffsetCheck()
        {
            //Arrange
            var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(1000);
            var unixTimeStampInSeconds = dateTimeOffset.ToUnixTimeSeconds();
            //Act
            var expDate = UnixTimeStamp.UnixTimeStampToDateTime(unixTimeStampInSeconds);
            //result
            expDate.Should().BeMoreThan(1.Days()).Before(DateTime.UtcNow);
        }
        [Fact]
        public void Pagination()
        {
            //Arrange
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization()
            {
                ConfigureMembers = true
            });
            var filter = fixture.Create<PaginationFilter>();
            var uri = fixture.Create<IUriService>();
            List<Products> product = new()
            {
                new Products()
            };
            //Act
            var uriService = uri.GetPageUri(filter, "test.com");
            var value = PaginationHelper.CreatePagedResponse(new InClassName<Products>(product, filter, 10, uri, "test.com"));
            //result
            uriService.Should().NotBeNull();
            uriService.AbsolutePath.Should().Be("/");
            value.Should().NotBeNull();
            value.TotalRecords.Should().Be(10);
        }
    }
}