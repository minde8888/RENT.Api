using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using FluentAssertions.Extensions;
using RENT.Data.Helpers;
using RENT.Data.Interfaces.IServices;
using RENT.Domain.Entities;

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
            //Arrange
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(1000);
            long unixTimeStampInSeconds = dateTimeOffset.ToUnixTimeSeconds();
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
            List<Products> product = new();
            product.Add(new Products());
            //Act
            var uriService = uri.GetPageUri(filter, "test.com");
            var value = PaginationHelper.CreatePagedReponse(product, filter, 10, uri, "test.com");
            //result
            uriService.Should().NotBeNull();
            uriService.AbsolutePath.Should().Be("/");
            value.Should().NotBeNull();
            value.TotalRecords.Should().Be(10);
        }
    }
}